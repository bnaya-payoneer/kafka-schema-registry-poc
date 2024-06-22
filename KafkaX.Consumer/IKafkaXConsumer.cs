using Avro.Specific;
using Confluent.Kafka;

namespace KafkaX;

public interface IKafkaXConsumer: IDisposable
{
    Task<ConsumeResult<Null, TValue>> ConsumeXAsync<TValue>(
                            CancellationToken cancellationToken)
                               where TValue : ISpecificRecord;
}