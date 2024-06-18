using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddSingleton((sp) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var kafkaUrls = cfg["Urls:Kafka"] ?? Environment.GetEnvironmentVariable("KAFKA_URLS") ?? "localhost:9092";

    return new ProducerConfig
    {
        BootstrapServers = kafkaUrls, // Kafka server address
        ClientId = "otel-demo-producer", // Identifier for the client
        Acks = Acks.All, // Wait for all in-sync replicas to acknowledge the message
        SecurityProtocol = SecurityProtocol.Plaintext,
        ApiVersionRequest = true,
    };
});

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
