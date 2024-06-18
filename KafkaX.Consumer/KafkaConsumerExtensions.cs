using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaX;

// TODO: check whether Stream result (value) is supported

public static class KafkaConsumerExtensions
{
    public static async Task<ConsumeResult<Ignore, TValue>> ConsumeXAsync<TValue>(
                                this IConsumer<Ignore, byte[]> consumer,
                                CancellationToken cancellationToken)
    {
        ConsumeResult<Ignore, byte[]> result = consumer.Consume(cancellationToken);
        // TODO: fetch the schema (scemaKey, scemaVersion)
        byte[] buffer = result.Message.Value;
        // TODO: get the identifier from the header
        // TODO: var item = Avro.Deserialize(schema, buffer)
        // var response = new ConsumeResult<Null, TValue>(item);
        // return response;
        throw new NotImplementedException();
    }
}
