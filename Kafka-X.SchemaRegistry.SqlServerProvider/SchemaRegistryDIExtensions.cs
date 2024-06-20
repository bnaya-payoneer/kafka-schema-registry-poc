using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaX;
public static class SchemaRegistryDIExtensions
{
    public static IServiceCollection AddSchemaRegistryRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<SchemaRegistryRepository>();
        var connectionString = configuration.GetConnectionString("SqlServer");

        services.AddDbContext<SchemaRegistryContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
}
