using Confluent.Kafka;
using KafkaXWebApp;
using static Confluent.Kafka.ConfigPropertyNames;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var cfg = builder.Configuration;
string kafkaUrls = cfg["Urls:Kafka"] ?? Environment.GetEnvironmentVariable("KAFKA_URLS") ?? "localhost:9092";

services.AddSchemaRegistryRepository(builder.Configuration);
services.AddKafkaXProducer(kafkaUrls);
services.AddKafkaXConsumer(kafkaUrls, $"demo: {Guid.NewGuid()}");
services.AddMemoryCache();
services.AddHostedService<ConsumerJob>();

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
