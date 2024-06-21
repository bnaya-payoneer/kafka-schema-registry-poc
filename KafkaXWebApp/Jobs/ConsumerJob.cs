using Confluent.Kafka;
using KafkaX;
using static KafkaX.Constants;

namespace KafkaXWebApp;

public class ConsumerJob : BackgroundService
{
    private readonly ILogger<ConsumerJob> _logger;
    private readonly IKafkaConsumer _consumer;

    public ConsumerJob(
        ILogger<ConsumerJob> logger,
        IKafkaConsumer consumer)
    {
        _logger = logger;
        _consumer = consumer;
        _consumer.Subscribe(TOPIC);

    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var data = await _consumer.ConsumeXAsync<Person>(TOPIC, stoppingToken);
            _logger.LogInformation("Consumed [{name}]: {code}", data.Name, data.Code);
        }
    }
}
