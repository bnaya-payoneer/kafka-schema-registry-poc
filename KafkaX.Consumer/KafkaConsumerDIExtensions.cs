using KafkaX;

namespace Microsoft.Extensions.DependencyInjection;

public static class KafkaConsumerDIExtensions
{
    public static IServiceCollection AddKafkaXConsumer(
                                    this IServiceCollection services)
    {
        services.AddScoped<IKafkaConsumer, KafkaConsumer>();
        return services;
    }
}
