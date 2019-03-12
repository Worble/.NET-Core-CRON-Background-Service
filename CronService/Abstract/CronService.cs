using Microsoft.Extensions.Hosting;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;
using static NCrontab.CrontabSchedule;

namespace CronService
{
    public abstract class CronService : BackgroundService
    {
        private readonly CrontabSchedule _schedule;
        private DateTime nextRun;

        protected CronService(string cron)
        {
            _schedule = Parse(cron, new ParseOptions { IncludingSeconds = true });
            nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var diff = nextRun - DateTime.Now;
                if (diff <= TimeSpan.Zero)
                {
                    await ExecuteTaskAsync().ConfigureAwait(false);
                    nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                diff = nextRun - DateTime.Now;
                await Task.Delay((int)diff.TotalMilliseconds, stoppingToken).ConfigureAwait(false);
            }
        }

        protected abstract Task ExecuteTaskAsync();
    }
}