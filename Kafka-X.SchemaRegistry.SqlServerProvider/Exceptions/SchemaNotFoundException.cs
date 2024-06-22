
namespace KafkaX;

[Serializable]
internal class SchemaNotFoundException : Exception
{
    private SchemaNotFoundException()
    {
    }

    private SchemaNotFoundException(string? message) : base(message)
    {
    }

    public SchemaNotFoundException(string key, int version) : base($"SchemaInfo not found for key: {key} and version: {version}")
    {
    }

    private SchemaNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}