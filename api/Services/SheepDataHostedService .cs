using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
namespace api.Services
{
    public class SheepDataHostedService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public SheepDataHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var excelExportService = scope.ServiceProvider.GetRequiredService<ExcelExportService>();
                var folderPath = Path.Combine("SheepData", DateTime.Now.ToString("yyyyMMddHHmm"));
                Directory.CreateDirectory(folderPath);
                await excelExportService.ExportSheepDataAsync();
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