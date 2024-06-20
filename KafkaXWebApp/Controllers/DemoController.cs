using KafkaX;
using Microsoft.AspNetCore.Mvc;
using static KafkaX.Constants;

namespace KafkaXWebApp.Controllers;
[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    private readonly ILogger<DemoController> _logger;
    private readonly IKafkaProducer _producer;

    public DemoController(
        ILogger<DemoController> logger,
        IKafkaProducer producer)
    {
        _logger = logger;
        _producer = producer;
    }

    [HttpPost]
    public async Task PostAsync([FromBody] PersonEntity payload)
    {
        var person = payload.FromEntity();
        await _producer.ProduceXAsync(TOPIC, person);
        _logger.LogInformation("Person [{name}]: {code}", person.Name, person.Code);
    }
}
