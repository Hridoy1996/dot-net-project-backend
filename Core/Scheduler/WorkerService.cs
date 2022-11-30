using Contract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using IApplicationLifetime = Microsoft.Extensions.Hosting.IApplicationLifetime;

namespace Infrastructure.Core.Scheduler
{
    public class WorkerService : BackgroundService
    {
        private const int generalDelay = 5 * 60 * 1000;
        private readonly IAppointmentManager _appointmentManager;
        private readonly IApplicationLifetime _applicationLifeTime;

        public WorkerService(IAppointmentManager appointmentManager, 
            IApplicationLifetime applicationLifetime)
        {
            _appointmentManager = appointmentManager;
            _applicationLifeTime = applicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(generalDelay, stoppingToken);
                    await SyncServiceStatus();
                }
            }
            catch (Exception)
            {
                _applicationLifeTime.StopApplication();
            }
            finally
            {

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
