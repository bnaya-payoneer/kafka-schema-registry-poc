using KafkaX;

namespace Microsoft.Extensions.DependencyInjection;

public static class KafkaProducerDIExtensions
{
    public static IServiceCollection AddKafkaXProducer(
                                    this IServiceCollection services)
    {
        services.AddScoped<IKafkaProducer, KafkaProducer>();
        return services;
    }
}
