using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElderlyService.Models;
using ElderlyService.Data;
using Microsoft.Extensions.Hosting;

namespace BackgroundTaskService.Library
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
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(24)); // Run the task every 24 hours
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); // Replace YourDbContext with your actual DbContext class

                // Get caregivers with expired subscriptions
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



