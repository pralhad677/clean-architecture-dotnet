using System.Data;
using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Data;
using Bookify.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace Bookify.Infrastructure.OutBox;

[DisallowConcurrentExecution]
public class ProcessOutBoxMessagesJob:IJob  //Quartz.Extensions.Hosting  nuget package
{
    private readonly JsonSerializerSettings _jsonSerializerSettings =new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };

    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<ProcessOutBoxMessagesJob> _logger;

    public ProcessOutBoxMessagesJob(ISqlConnectionFactory sqlConnectionFactory, IPublisher publisher, IDateTimeProvider dateTimeProvider, OutboxOptions outboxOptions, ILogger<ProcessOutBoxMessagesJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _outboxOptions = outboxOptions;
        _logger = logger;
    }

    public async  Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting outbox messages job");
        using var connection = _sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();

        var outBoxMessages = await GetOutBoxMessageAsync(connection, transaction);

        foreach (var outBoxMessage in outBoxMessages)
        {
            Exception? exception = null;
            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvents>(
                    outBoxMessage.Content,
                    _jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,  "Exception while processing outbox message  {MessageId} ",outBoxMessage.Id);
            }

            await UpdateOutBoxMessageAsync(connection, transaction, outBoxMessage,exception);
        }
        transaction.Commit();
        _logger.LogInformation("Finished outbox messages job");
    }

    private async Task UpdateOutBoxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutBoxMessageResponse outBoxMessage,
        Exception? exception)
    {
        const string sql =@" 
                           update outbox messages
                            set processed_on_utc =@ProcessedOnUtc
                            error=@Error,
                            where id=@Id;
                           " ;

        await connection.ExecuteAsync(
            sql,
            new
            {
         outBoxMessage.Id,
         ProcessOnUtc = _dateTimeProvider.UtcNow,
         Error = exception?.ToString(),
            },
            transaction: transaction);

    }

    private async Task<IReadOnlyList<OutBoxMessageResponse>> GetOutBoxMessageAsync(
        IDbConnection connection, IDbTransaction transaction)
    {
        var sql= $"""
                  Select id,content
                  from OutBoxMessages
                  where processed_on_utc is null
                  order by occured_on_utc
                  limit {_outboxOptions.BatchSize}
                  for update
                  """;

        var outboxMessages =  await connection.QueryAsync<OutBoxMessageResponse>(sql, transaction: transaction);
        return outboxMessages.ToList();
    }

    internal sealed record OutBoxMessageResponse(Guid Id, string Content);
}