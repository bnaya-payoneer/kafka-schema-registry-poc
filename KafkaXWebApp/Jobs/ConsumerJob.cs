using Confluent.Kafka;
using KafkaX;
using static Confluent.Kafka.ConfigPropertyNames;
using static KafkaX.Constants;

namespace KafkaXWebApp;

public sealed class ConsumerJob : IHostedLifecycleService
{
    private readonly ILogger<ConsumerJob> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private Task _executing = Task.CompletedTask;

    public ConsumerJob(
        ILogger<ConsumerJob> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    private async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var factory = scope.ServiceProvider.GetRequiredService<IKafkaXConsumerFactory>();
            using IKafkaXConsumer consumer = factory.Subscribe(TOPIC);
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await consumer.ConsumeXAsync<Person>(stoppingToken);
                var data = result.Message.Value;
                _logger.LogInformation("Consumed [{name}]: {code}", data.Name, data.Code);
            }
        }
    }

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    Task IHostedLifecycleService.StartedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    Task IHostedLifecycleService.StartingAsync(CancellationToken cancellationToken)
    {
        _executing = Task.Factory.StartNew(() => ExecuteAsync(cancellationToken), 
                                    TaskCreationOptions.LongRunning)
                               .Unwrap();
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    Task IHostedLifecycleService.StoppedAsync(CancellationToken cancellationToken)
    {
        _executing = Task.CompletedTask;
        return Task.CompletedTask;
    }

    Task IHostedLifecycleService.StoppingAsync(CancellationToken cancellationToken)
    {
        return _executing;
    }
}
