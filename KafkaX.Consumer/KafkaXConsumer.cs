using Avro;
using Confluent.Kafka;

namespace KafkaX;

public class KafkaXConsumer : IKafkaXConsumer, IKafkaXConsumerFactory
{
    private readonly ISchemaStorageProvider _storageProvider;
    private readonly IConsumer<Null, byte[]> _consumer;

    public KafkaXConsumer(
        ISchemaStorageProvider storageProvider,
        IConsumer<Null, byte[]> consumer)
    {
        _storageProvider = storageProvider;
        _consumer = consumer;
    }


    async Task<ConsumeResult<Null, TValue>> IKafkaXConsumer.ConsumeXAsync<TValue>(
                                CancellationToken cancellationToken)
    {
        ConsumeResult<Null, byte[]> message = _consumer.Consume(cancellationToken);
        var headers = message.Message.Headers;
        byte[] data = message.Message.Value;
        var versionBuffer = headers.GetLastBytes("schema-version");
        int version = BitConverter.ToInt32(versionBuffer);
        Schema schema = await _storageProvider.GetSchemaAsync<TValue>(version);
        var result = schema.DeserializeFromAvro<TValue>(data);
        return new ConsumeResult<Null, TValue>()
        {
            Message = new Message<Null, TValue>()
            {
                Value = result,
                Headers = headers,
            },
            Topic = message.Topic,
            Partition = message.Partition,
            Offset = message.Offset,
        };
    }

    void IDisposable.Dispose()
    {
        _consumer.Unsubscribe();
    }

    IKafkaXConsumer IKafkaXConsumerFactory.Subscribe(string topic)
    {
        _consumer.Subscribe(topic);
        return this;
    }
}
