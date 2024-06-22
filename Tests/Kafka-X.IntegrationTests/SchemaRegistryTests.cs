// Ignore Spelling: Sql

using Cocona;
using Cocona.Builder;
using KafkaX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kafka_X.IntegrationTests;

public class SchemaRegistryTests
{
    private readonly ISchemaStorageProvider _storeProvider;

    public SchemaRegistryTests()
    {
        CoconaAppBuilder builder = CoconaApp.CreateBuilder();
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", true, true);
        //.AddJsonFile($"appsettings.{environmentName}.json", true, true);

        var services = builder.Services;
        services.AddSchemaRegistryRepository(builder.Configuration);
        services.AddMemoryCache();
        var sp = services.BuildServiceProvider();
        _storeProvider = sp.GetRequiredService<ISchemaStorageProvider>();

    }

    [Fact]
    public async Task SqlRegistryStorageTest()
    {
        var p = new PersonCls
        {
            Name = "Sally",
            Code = 1234
        };

        Avro.Schema schema = PersonCls.SCHEMA;
        var schema1 = await _storeProvider.GetOrAddSchemaAsync<PersonCls>();
        var schema2 = await _storeProvider.GetSchemaAsync<PersonCls>();

        Assert.NotNull(schema?.ToString());
        Assert.NotEmpty(schema.ToString());
        Assert.Equal(schema.ToString(), schema1.ToString());
        Assert.Equal(schema2.ToString(), schema1.ToString());
    }
}