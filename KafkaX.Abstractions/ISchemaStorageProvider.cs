using Avro;
using Avro.Specific;

namespace KafkaX;

public interface ISchemaStorageProvider
{
    Task<Schema> GetSchemaAsync<T>(int version = -1)
        where T : ISpecificRecord;
    Task<Schema> GetOrAddSchemaAsync<T>(int version = -1)
        where T : ISpecificRecord;
}
