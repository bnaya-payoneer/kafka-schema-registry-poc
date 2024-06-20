using Confluent.Kafka;
using KafkaX;
using Microsoft.AspNetCore.Mvc;
using static KafkaX.Constants;

namespace KafkaXWebApp.Controllers;
[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    private readonly ILogger<DemoController> _logger;
    private readonly ProducerConfig _config;

    public DemoController(
        ILogger<DemoController> logger,
        ProducerConfig config)
    {
        _logger = logger;
        _config = config;
    }

    [HttpGet]
    public async Task PostAsync([FromBody] Foo payload)
    {
        using IProducer<Ignore, byte[]> producer = new ProducerBuilder<Ignore, byte[]>(_config)
                                    //.SetValueSerializer(new KafkaJsonSerializer<OrderMessage>())
                                    //.SetValueSerializer(Serializers.Utf8)
                                    .Build();
        //.BuildWithInstrumentation();
        //await producer.ProduceXAsync(TOPIC, payload);
        _logger.LogInformation("Produce [{id}]: {product}", payload.Id, payload.Product);
    }
}
