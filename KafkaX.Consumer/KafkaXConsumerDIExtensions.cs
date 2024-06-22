using Confluent.Kafka;
using KafkaX;

namespace Microsoft.Extensions.DependencyInjection;

public static class KafkaXConsumerDIExtensions
{
    public static IServiceCollection AddKafkaXConsumer(
                                    this IServiceCollection services,
                                    string urls,
                                    string groupId)
    {
        services.AddSingleton((sp) =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = urls, // Kafka server address
                ClientId = "registry-demo", // Identifier for the client
                GroupId = groupId, // Identifier for the consumer group
                Acks = Acks.All, // Wait for all in-sync replicas to acknowledge the message
                SecurityProtocol = SecurityProtocol.Plaintext,
                ApiVersionRequest = true,
                AllowAutoCreateTopics = true,
                // TODO: ack policy
            };
            return config;
        });

        services.AddScoped<IKafkaXConsumerFactory, KafkaXConsumer>();
        services.AddScoped(sp =>
        {
            var config = sp.GetRequiredService<ConsumerConfig>();
            return new ConsumerBuilder<Null, byte[]>(config).Build();
        });
        return services;
    }
}
