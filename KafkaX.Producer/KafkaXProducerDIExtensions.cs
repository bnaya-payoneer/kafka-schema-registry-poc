using Confluent.Kafka;
using KafkaX;

namespace Microsoft.Extensions.DependencyInjection;

public static class KafkaXProducerDIExtensions
{
    public static IServiceCollection AddKafkaXProducer(
                                    this IServiceCollection services,
                                    string urls)
    {
        services.AddSingleton((sp) =>
        {
            var config = new ProducerConfig
            {
                BootstrapServers = urls, // Kafka server address
                ClientId = "registry-demo", // Identifier for the client
                Acks = Acks.All, // Wait for all in-sync replicas to acknowledge the message
                SecurityProtocol = SecurityProtocol.Plaintext,
                ApiVersionRequest = true,
            };
            return config;
        });

        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<ProducerConfig>();
            return new ProducerBuilder<Null, byte[]>(config).Build();
        });

        services.AddScoped<IKafkaXProducer, KafkaXProducer>();
        return services;
    }
}
