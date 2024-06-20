using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: understand the Message.Key

namespace KafkaX;
public class KafkaProducer
{
    private readonly ISchemaStorageProvider _storageProvider;

    public KafkaProducer(ISchemaStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<DeliveryResult<Ignore, TValue>> ProduceXAsync<TValue>(
        this IProducer<Ignore, byte[]> producer,
        string topic,
        TValue payload,
        int version = -1,
        CancellationToken cancellationToken = default(CancellationToken))
        //where TValue : IKafkaIdentifier
    {
        var schema = _storageProvider.GetOrAddSchemaAsync<TValue>(version);
        // TODO: var item = Avro.Serialize(schema, buffer)
        //var message = new Message<Ignore, string>
        //{
        //    Value = item,
        //    //Headers =
        //};
        //var response = await producer.ProduceAsync(topic, message, cancellationToken);
        //return response;
        throw new NotImplementedException();
    }
}
