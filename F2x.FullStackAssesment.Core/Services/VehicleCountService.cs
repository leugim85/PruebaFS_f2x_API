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
        private readonly IMapper mapper;

        const int maxConcurrency = 2;

        public VehicleCountService(IMicroClientHelper microClientHelper,
            IConfigProvider configProvider,
            IVehicleCounterRepository vehicleCounterRepository,
            IVehicleCounterQueryHistoryRepository vehicleCounterQueryHistoryRepository,
            IVehicleCounterWithAmountRepository vehicleCounterWithAmountRepository,
            IMapper mapper)
        {
            this.microClientHelper = microClientHelper;
            this.configProvider = configProvider;
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
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
                await Task.WhenAll(tasks);
            }
        }

        public async Task GetDataVehicleCounter(string date, string token)            
        {           
            SaveDataVehicleCounter(date, token).GetAwaiter();
            SaveDataVehicleCounterWithAmount(date, token).GetAwaiter();
        }

        public async Task<GeneralSummaryDto> GetSummary(string station)
        {
            List<Expression<Func<VehicleCounterInformation, bool>>> filters = ValidatePredicateGetCollectedvehicleCounter(station);
            GeneralSummaryDto summary = await GetSummary(filters);

            return summary;
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

        private List<Expression<Func<VehicleCounterInformation, bool>>> ValidatePredicateGetCollectedvehicleCounter(string station)
        {
            List<Expression<Func<VehicleCounterInformation, bool>>> filters = new List<Expression<Func<VehicleCounterInformation, bool>>>();

            if (!string.IsNullOrEmpty(station))
            {
                Expression<Func<VehicleCounterInformation, bool>> predicate = x => x.Station.Contains(station);
                filters.Add(predicate);
            }
            
            return filters;
        }

        private async Task<GeneralSummaryDto> GetSummary(
        List<Expression<Func<VehicleCounterInformation, bool>>> filters = null,
        Func<IQueryable<VehicleCounterInformation>, IOrderedQueryable<VehicleCounterInformation>> orderBy = null,
          string includeProperties = "")
        {
            GeneralSummaryDto generalSummary = new GeneralSummaryDto();

            var result = await vehicleCounterRepository.GetAllAsyncWithFilters(filters, orderBy, includeProperties);
            List<(VehicleCounterInformation item, double amount)> mergeItems = new List<(VehicleCounterInformation, double)>();
            foreach ( var item in result ) 
            {
              List<Expression<Func<VehicleCounterWithAmount, bool>>> filtersForAmount = ValidatePredicateGetCollectedvehicleAmount(item.Station,item.Hour, item.Date);
               var temporalIems= await vehicleCounterWithAmountRepository.GetAllAsyncWithFilters(filtersForAmount);
               mergeItems.Add((item,temporalIems.Sum(t => t.Amount)));                  
            }
            generalSummary.TotalCarsGeneral = result.Sum(i => i.Quantity);
            generalSummary.TotalAmountGeneral = mergeItems.Sum(i => i.amount);
            generalSummary.VehicleCounterSummaryList = GetStationSummary(mergeItems);

            return generalSummary;
        }

        private List<Expression<Func<VehicleCounterWithAmount, bool>>> ValidatePredicateGetCollectedvehicleAmount(string station, string? hour, DateTime? date)
        {
            List<Expression<Func<VehicleCounterWithAmount, bool>>> filters = new List<Expression<Func<VehicleCounterWithAmount, bool>>>();

            if (!string.IsNullOrEmpty(station))
            {
                Expression<Func<VehicleCounterWithAmount, bool>> predicate = x => x.Station.Contains(station);
                filters.Add(predicate);
            }
            if (!(date is null))
            {
                Expression<Func<VehicleCounterWithAmount, bool>> predicate = x => x.Date.Equals(date);
                filters.Add(predicate);
            }
            if (!(hour is null))
            {
                Expression<Func<VehicleCounterWithAmount, bool>> predicate = x => x.Hour.Equals(hour);
                filters.Add(predicate);
            }

            return filters;
        }

        private List<StationSummaryDto> GetStationSummary(List<(VehicleCounterInformation, double)> result) 
        {
            List<StationSummaryDto> stationSummaryDto = new List<StationSummaryDto>();
            var resultGroupedByStation = result.GroupBy(i => new { i.Item1.Station });
            var resultGroupedByDate = result.GroupBy(i => new {i.Item1.Date});

            stationSummaryDto = resultGroupedByStation.Select(e => new StationSummaryDto
            {
                Station = e.Key.Station,
                TotalAmount = e.Sum(f => f.Item2),
                VehicleCount= e.Sum(f => f.Item1.Quantity),
                SummaryByDates = resultGroupedByDate.Select(d =>  new SummaryByDateDto
                {
                    Date = new DateOnly(d.Key.Date.Year, d.Key.Date.Month, d.Key.Date.Day),
                    VehicleCountByDate = d.Sum(r => r.Item1.Quantity),
                    TotalAmountByDate = d.Sum(r => r.Item2)
                }).ToList()

            }).ToList();

            return stationSummaryDto;
        }        

        private async Task<List<string>> GetPendingDates()
        {
            var lastDate = await GetLastDate();

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
