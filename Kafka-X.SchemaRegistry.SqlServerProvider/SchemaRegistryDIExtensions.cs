using KafkaX;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class SchemaRegistryDIExtensions
{
    public static IServiceCollection AddSchemaRegistryRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISchemaStorageProvider, SchemaRegistryRepository>();
        var connectionString = configuration.GetConnectionString("SqlServer");

        services.AddDbContext<SchemaRegistryContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
}
