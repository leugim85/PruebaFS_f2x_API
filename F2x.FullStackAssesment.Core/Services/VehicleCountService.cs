using F2xFullStackAssesment.Core.Helpers;
using F2xFullStackAssesment.Infraestructure.IResources;
using System.Collections.Generic;
using System.Threading.Tasks;
using F2x.FullStackAssesment.Core.IServices;
using F2xFullStackAssesment.Core.Dtos.Response;
using System;
using F2xFullStackAssesment.Domain.IRepository;
using AutoMapper;
using F2xF2xFullStackAssesment.Domain.Entities;
using F2x.FullStackAssesment.Core.Dtos.Request;
using System.Linq.Expressions;
using F2x.FullStackAssesment.Core.Dtos.Response;
using System.Linq;
using F2x.FullStackAssesment.Core.Dtos.VehicleCounterApi.Response;
using F2x.FullStackAssesment.Domain.Entities;
using System.Threading;

namespace F2x.FullStackAssesment.Core.Services
{
    public class VehicleCountService : IVehicleCountService
    {
        private readonly IMicroClientHelper microClientHelper;
        private readonly IConfigProvider configProvider;
        private readonly IVehicleCounterRepository vehicleCounterRepository;
        private readonly IVehicleCounterQueryHistoryRepository vehicleCounterQueryHistoryRepository;
        private readonly IVehicleCounterWithAmountRepository vehicleCounterWithAmountRepository;
        private readonly IMessagesProvider messagesProvider;
        private readonly IMapper mapper;

        const int maxConcurrency = 2;

        public VehicleCountService(IMicroClientHelper microClientHelper,
            IConfigProvider configProvider,
            IVehicleCounterRepository vehicleCounterRepository,
            IVehicleCounterQueryHistoryRepository vehicleCounterQueryHistoryRepository,
            IVehicleCounterWithAmountRepository vehicleCounterWithAmountRepository,
            IMessagesProvider messagesProvider,
            IMapper mapper)
        {
            this.microClientHelper = microClientHelper;
            this.configProvider = configProvider;
            this.messagesProvider = messagesProvider;
            this.vehicleCounterRepository = vehicleCounterRepository;
            this.vehicleCounterQueryHistoryRepository = vehicleCounterQueryHistoryRepository;
            this.vehicleCounterWithAmountRepository = vehicleCounterWithAmountRepository;
            this.mapper = mapper;
        }

        public async Task ProcessdataFromCounterApi()
        {
            List<string> pendigDates = new List<string>();
            pendigDates.AddRange(await GetPendingDates());
            string token = await microClientHelper.GetTokenMicroServiceAsync();
            using (var throttler = new SemaphoreSlim(maxConcurrency))
            {
                var tasks = new List<Task>();
                foreach (var date in pendigDates)
                {
                    await throttler.WaitAsync();
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            GetDataVehicleCounter(date, token).GetAwaiter();
                        }
                        finally
                        {
                            throttler.Release(); 
                        }
                    }));
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
                await Task.WhenAll(tasks);
            }
        }

        public async Task GetDataVehicleCounter(string date, string token)            
        {           
            SaveDataVehicleCounter(date, token).GetAwaiter();
            SaveDataVehicleCounterWithAmount(date, token).GetAwaiter();
        }

        public async Task<VehiclesCounterCollectedPaginated> GetDataVehicleCounterPaginated(VehiclesInformationPaginatedDto vehiclesInformationPaginated)
        {
            int skip = (vehiclesInformationPaginated.PageNumber - 1) * vehiclesInformationPaginated.Take;
            List<Expression<Func<VehicleCounterInformation, bool>>> filters = ValidatePredicateGetCollectedvehicleCounter(vehiclesInformationPaginated.Station, vehiclesInformationPaginated.StartDate, vehiclesInformationPaginated.EndtDate);
            List<VehiclesCounterDataDto> items = (await GetAllPagedAsync(vehiclesInformationPaginated.Take, skip, filters)).ToList();
            int totalRows = await CountAsync(filters);
            return new VehiclesCounterCollectedPaginated(vehiclesInformationPaginated.PageNumber, vehiclesInformationPaginated.Take, totalRows, items);
        }

        public async Task<DateOnly> GetLastDate()
        {
            var result = await vehicleCounterQueryHistoryRepository.GetAllAsync();
            var lastDate = result.OrderByDescending(r => r.Date).Select(r => r.Date).FirstOrDefault();
            return new DateOnly(lastDate.Year, lastDate.Month, lastDate.Day);
        }

        #region Private Metohds   

        private async Task SaveDataVehicleCounterWithAmount(string date, string token)
        {
            List<VehicleCounterWithAmountDto> resp = await microClientHelper.GetDataAsync<List<VehicleCounterWithAmountDto>>($"{configProvider.VehiclesCounterValuePath}{date}", token);
            if (resp is null)
            {
                return;
            }

            List<VehicleCounterWithAmount> vehiclesCountersWithAmount = mapper.Map<List<VehicleCounterWithAmount>>(resp);
            foreach (var vehicleCounter in vehiclesCountersWithAmount)
            {
                vehicleCounter.Date = DateTime.Parse(date);
            }
            await vehicleCounterWithAmountRepository.AddRangeAsync(vehiclesCountersWithAmount);
        }
        private async Task SaveDataVehicleCounter(string date, string token)
        {
            List<VehiclesCounterInformationDto> resp = await microClientHelper.GetDataAsync<List<VehiclesCounterInformationDto>>($"{configProvider.VehiclesCounterPath}{date}", token);
            List<VehicleCounterInformation> vehiclesCountersData = mapper.Map<List<VehicleCounterInformation>>(resp);
            if (resp is null)
            {
                SaveDateQueryHistory(date, 0).GetAwaiter();
                return;
            }

            SaveDateQueryHistory(date, resp.Count()).GetAwaiter();
            foreach (var vehicleCounter in vehiclesCountersData)
            {
                vehicleCounter.Date = DateTime.Parse(date);
            }
            vehicleCounterRepository.AddRangeAsync(vehiclesCountersData).GetAwaiter();
          }

        private async Task SaveDateQueryHistory(string date, int itemsQuantity)
        {
            VehicleCounterQueryHistoryDto vehicleCounterQueryHistoryDto = new VehicleCounterQueryHistoryDto();
            vehicleCounterQueryHistoryDto.Date = DateTime.Parse(date);
            vehicleCounterQueryHistoryDto.Quantity = itemsQuantity;
            VehicleCounterQueryHistory vehicleCounterQueryHistory = mapper.Map<VehicleCounterQueryHistory>(vehicleCounterQueryHistoryDto);
            vehicleCounterQueryHistoryRepository.AddAsync(vehicleCounterQueryHistory).GetAwaiter();
        }

        private List<Expression<Func<VehicleCounterInformation, bool>>> ValidatePredicateGetCollectedvehicleCounter(string station, DateTime? startDate, DateTime? endDate)
        {
            List<Expression<Func<VehicleCounterInformation, bool>>> filters = new List<Expression<Func<VehicleCounterInformation, bool>>>();

            if (!string.IsNullOrEmpty(station))
            {
                Expression<Func<VehicleCounterInformation, bool>> predicate = x => x.Station.Contains(station);
                filters.Add(predicate);
            }
            if (!(startDate is null))
            {
                Expression<Func<VehicleCounterInformation, bool>> predicate = x => x.Date >= startDate;
                filters.Add(predicate);
            }
            if (!(endDate is null))
            {
                Expression<Func<VehicleCounterInformation, bool>> predicate = x => x.Date <= endDate;
                filters.Add(predicate);
            }

            return filters;
        }

        private async Task<IEnumerable<VehiclesCounterDataDto>> GetAllPagedAsync(
        int take,
        int skip,
        List<Expression<Func<VehicleCounterInformation, bool>>> filters = null,
        Func<IQueryable<VehicleCounterInformation>, IOrderedQueryable<VehicleCounterInformation>> orderBy = null,
          string includeProperties = "")
        {
            return mapper.Map<IEnumerable<VehiclesCounterDataDto>>(await vehicleCounterRepository.GetAllPagedAsync(take, skip, filters, orderBy, includeProperties));
        }

        private async Task<List<string>> GetPendingDates()
        {
            var lastDate = await GetLastDate();
            //var lastDate = new DateOnly(2021,07,31);

            lastDate = lastDate.AddDays(1);
            string CustomFormatDate = "yyyy-MM-dd";
            List<string> dates = new List<string>();
            var endDate = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(-1));

            for (DateOnly date = lastDate; date <= endDate; date = date.AddDays(1))
            {
                var lastDateString = date.ToString(CustomFormatDate);
                dates.Add(lastDateString);
            }

            return dates;
        }

        private async Task<int> CountAsync(List<Expression<Func<VehicleCounterInformation, bool>>> filters = null)
        {
            return await vehicleCounterRepository.CountAsync(filters);
        }
        #endregion

    }
}
