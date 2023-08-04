using DBContextF2xF2xFullStackAssesment.Domain;
using F2x.FullStackAssesment.Core.IServices;
using F2xFullStackAssesment.Core.Helpers;
using F2xFullStackAssesment.Infraestructure.IResources;
using F2xFullStackAssesment.Infraestructure.Resources;

namespace F2xFullStackAssesment.TimerWorker
{
    public class Worker : IHostedService, IDisposable
    {
        private readonly IConfigProvider configProvider;
        private readonly IVehicleCountService vehicleCountService;
        private readonly IMicroClientHelper microClientHelper;
        private bool disposedValue;
        private Timer timer;


        public Worker(IConfigProvider configProvider,
            IVehicleCountService vehicleCountService,
            IMicroClientHelper microClientHelper)
            
        {
            this.configProvider = configProvider;
            this.vehicleCountService = vehicleCountService;
            this.microClientHelper = microClientHelper;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            double executionSeconds = GetSecondsToExecution();
            timer = new Timer(DoWork, null, TimeSpan.FromSeconds(executionSeconds), TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private double GetSecondsToExecution()
        {
            DateTime dateNow = DateTime.UtcNow.GetColombiaDateNow();
            DateTime executionDate = new DateTime(dateNow.Year, dateNow.Month, 3, 13,33, 0).AddDays(1);

            return (executionDate - dateNow).TotalSeconds;
        }

        private async void DoWork(object? state)
        {
            try
            {
                vehicleCountService.ProcessdataFromCounterApi().GetAwaiter(); 
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                timer.Change(TimeSpan.FromSeconds(3600), TimeSpan.FromSeconds(1));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    timer.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

       
    }
}