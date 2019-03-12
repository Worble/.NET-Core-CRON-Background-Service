using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CronService.Services
{
    public class TestService : CronService
    {
        private const string Path = @".\test.txt";
        private readonly ILogger<TestService> _logger;

        public TestService(ILogger<TestService> logger, Configuration config) : base(config.Cron)
        {
            _logger = logger;
        }

        protected override Task ExecuteTaskAsync()
        {
            _logger.LogInformation($"Worker running at: {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}