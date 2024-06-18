namespace KafkaX;

public interface ISchemaStorageProvider
{
    //Schema GetSchema (string Key, int version = -1);
    Schema GetOrAddSchema (string Key, ProvideNewSchema callback, int version = -1);
}
