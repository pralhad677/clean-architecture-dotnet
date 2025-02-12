using Microsoft.Extensions.Options;
using Quartz;

namespace Bookify.Infrastructure.OutBox;

public class ProcessOutBoxMessagesJobSetup:IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions;

    public ProcessOutBoxMessagesJobSetup(IOptions<OutboxOptions> outboxOptions)
    {
        _outboxOptions = outboxOptions.Value;
    }

    public void Configure(QuartzOptions options)
    {
        const string jobName=nameof(ProcessOutBoxMessagesJob);
        options.AddJob<ProcessOutBoxMessagesJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure.ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()));
    }
}