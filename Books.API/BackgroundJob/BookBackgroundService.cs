using Books.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Books.API.BackgroundJob
{
    public class BookBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BookBackgroundService> _logger;

        /// <summary>
        /// Can't inject less scoped IBooksServive directly in constructor. IHostedService Lifetime is Singleton
        /// Error :  Cannot consume scoped service 'Books.API.Services.IBooksRepository' from singleton (HostedService is singleton)
        /// 
        /// Implemented workaround for that below
        /// </summary>
        /// <param name="booksServive"></param>
        /// <param name="logger"></param>
        public BookBackgroundService(IServiceProvider serviceProvider, ILogger<BookBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopeService = scope.ServiceProvider.GetRequiredService<IBooksServive>();

                    var result = await scopeService.GetBooksAsync();

                    _logger.LogInformation("BackgroundService executed data sucessfully --- " + result.Count());

                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Better than timer if you want application do its task after every 10 seconds.

                    _logger.LogInformation("------------BackgroundService re-started sucessfully after 10 seconds !----------------------------------");
                }

            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BackgroundService started sucessfully !");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BackgroundService stopped sucessfully !");
            return base.StopAsync(cancellationToken);
        }
    }
}
