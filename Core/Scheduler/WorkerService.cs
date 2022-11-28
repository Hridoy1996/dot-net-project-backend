using Contract;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Core.Scheduler
{
    public class WorkerService : BackgroundService
    {
        private const int generalDelay = 5 * 60 * 1000;
        private readonly IAppointmentManager _appointmentManager;

        public WorkerService(IAppointmentManager appointmentManager)
        {
            _appointmentManager = appointmentManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(generalDelay, stoppingToken);
                await SyncServiceStatus();
            }
        }

        private async Task SyncServiceStatus()
        {
            try
            {
                await _appointmentManager.SyncServiceStatusAsync();
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}
