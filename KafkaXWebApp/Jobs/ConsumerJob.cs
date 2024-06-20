using Confluent.Kafka;
using KafkaX;
using static KafkaX.Constants;

namespace KafkaXWebApp;

public class ConsumerJob : BackgroundService
{
    private readonly ILogger<ConsumerJob> _logger;
    private readonly IConsumer<Ignore, byte[]> _consumer;

    public ConsumerJob(
        ILogger<ConsumerJob> logger,
        ConsumerConfig config)
    {
        _logger = logger;
        _consumer = new ConsumerBuilder<Ignore, byte[]>(config).Build();
        _consumer.Subscribe(TOPIC);

    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await _consumer.ConsumeXAsync<Foo>(stoppingToken);
            var data = result.Message.Value;
            _logger.LogInformation("Consumed [{id}]: {product}", data.Id, data.Product);
        }
    }

    public override void Dispose()
    {
        _consumer.Dispose();
    }
}
