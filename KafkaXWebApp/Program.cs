using Confluent.Kafka;
using static Confluent.Kafka.ConfigPropertyNames;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddSingleton((sp) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var kafkaUrls = cfg["Urls:Kafka"] ?? Environment.GetEnvironmentVariable("KAFKA_URLS") ?? "localhost:9092";

    var config = new ProducerConfig
    {
        BootstrapServers = kafkaUrls, // Kafka server address
        ClientId = "otel-demo-producer", // Identifier for the client
        Acks = Acks.All, // Wait for all in-sync replicas to acknowledge the message
        SecurityProtocol = SecurityProtocol.Plaintext,
        ApiVersionRequest = true,
    };
    var producer =  new ProducerBuilder<Null, byte[]>(config).Build();
    return producer;
});
services.AddSingleton((sp) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var kafkaUrls = cfg["Urls:Kafka"] ?? Environment.GetEnvironmentVariable("KAFKA_URLS") ?? "localhost:9092";

    var config = new ConsumerConfig
    {
        BootstrapServers = kafkaUrls, // Kafka server address
        ClientId = "otel-demo-producer", // Identifier for the client
        Acks = Acks.All, // Wait for all in-sync replicas to acknowledge the message
        SecurityProtocol = SecurityProtocol.Plaintext,
        ApiVersionRequest = true,
    };
    var consumer = new ConsumerBuilder<Null, byte[]>(config).Build();
    return consumer;
});
services.AddSchemaRegistryRepository(builder.Configuration);
services.AddKafkaXProducer();
services.AddKafkaXConsumer();
services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
