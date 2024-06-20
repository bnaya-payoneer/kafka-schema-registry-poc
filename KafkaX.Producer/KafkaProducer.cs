using Avro.Specific;
using Confluent.Kafka;
using System.Text;

namespace KafkaX;

public class KafkaProducer : IKafkaProducer
{
    private readonly ISchemaStorageProvider _storageProvider;
    private readonly IProducer<Null, byte[]> _producer;

    public KafkaProducer(
        ISchemaStorageProvider storageProvider,
        IProducer<Null, byte[]> producer)
    {
        _storageProvider = storageProvider;
        _producer = producer;
    }

    public async Task<DeliveryResult<Null, byte[]>> ProduceXAsync<TValue>(
        string topic,
        TValue payload,
        int version = -1,
        CancellationToken cancellationToken = default(CancellationToken))
        where TValue : ISpecificRecord
    {
        var schema = await _storageProvider.GetOrAddSchemaAsync<TValue>(version);
        var buffer = payload.SerializeToAvro();

        var message = new Message<Null, byte[]>
        {
            Value = buffer
        };
        message.Headers.Add("schema-key", Encoding.UTF8.GetBytes(schema.Identifier.SchemaKey));
        message.Headers.Add("schema-version", BitConverter.GetBytes(schema.Identifier.SchemaVersion));
        DeliveryResult<Null, byte[]> response =
            await _producer.ProduceAsync(topic, message, cancellationToken);
        return response;
    }
}
