using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElderlyService.Models;
using ElderlyService.Data;
using Microsoft.Extensions.Hosting;

namespace ElderlyService
{
    public class BackgroundTaskService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public BackgroundTaskService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // This method will be called when the application starts
            return Task.CompletedTask;
        }

        public void InitializeTimer()
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(24));
        }

        private void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var expiredCaregivers = dbContext.Caregivers
                    .Where(c => c.Valid == true && c.EndSubscribe.HasValue && c.EndSubscribe <= DateTime.Now)
                    .ToList();

                foreach (var caregiver in expiredCaregivers)
                {
                    caregiver.Valid = false;
                    dbContext.Caregivers.Update(caregiver);
                }

                dbContext.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
