using Avro.Specific;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace KafkaX;

public class KafkaConsumer : IKafkaConsumer
{

    private readonly ISchemaStorageProvider _storageProvider;
    private readonly IConsumer<Null, byte[]> _consumer;

    public KafkaConsumer(ISchemaStorageProvider schemaStorageProvider, IConsumer<Null, byte[]> consumer)
    {
        _storageProvider = schemaStorageProvider;
        _consumer = consumer;
    }

    public void Subscribe(string topic)
    {
        _consumer.Subscribe(topic);
    }

    public async Task<TValue> ConsumeXAsync<TValue>(
        string topic,
        CancellationToken cancellationToken = default)
        where TValue : ISpecificRecord
    {
        var consumeResult = _consumer.Consume(cancellationToken);
        var schemaVersion = BitConverter.ToInt32(consumeResult.Message.Headers.GetLastBytes("schema-version"));
        var schema = await _storageProvider.GetOrAddSchemaAsync<TValue>(schemaVersion);
        var avroSchema = Avro.Schema.Parse(Encoding.UTF8.GetString(schema.Definition))!;    
        var buffer = consumeResult.Message.Value;
        var payload = buffer.DeserializeFromAvro<TValue>(avroSchema);
        _consumer.Close();

        return payload;
    }
}
