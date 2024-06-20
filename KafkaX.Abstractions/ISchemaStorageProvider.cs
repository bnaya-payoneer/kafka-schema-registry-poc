namespace KafkaX;

public interface ISchemaStorageProvider
{
    Task<Schema> GetSchemaAsync(string key, int version = -1);
    Task<Schema> GetOrAddSchemaAsync(string key, ProvideNewSchema callback, int version = -1);
    Task<Schema> GetOrAddSchemaAsync<T>(int version = -1);
}
