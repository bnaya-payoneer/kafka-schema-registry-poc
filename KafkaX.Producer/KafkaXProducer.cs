using Avro;
using Avro.Specific;
using Confluent.Kafka;
using System.Text;

namespace KafkaX;

public class KafkaXProducer : IKafkaXProducer
{
    private readonly ISchemaStorageProvider _storageProvider;
    private readonly IProducer<Null, byte[]> _producer;

    public KafkaXProducer(
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
        Schema schema = await _storageProvider.GetOrAddSchemaAsync<TValue>(version);
        var buffer = schema.SerializeToAvro(payload);

        var message = new Message<Null, byte[]>
        {
            Value = buffer,
            Headers = new Headers()
        };
        message.Headers.Add("schema-key", Encoding.UTF8.GetBytes(typeof(TValue).FullName!));
        message.Headers.Add("schema-version", BitConverter.GetBytes(version));
        DeliveryResult<Null, byte[]> response =
            await _producer.ProduceAsync(topic, message, cancellationToken);
        return response;
    }
}
