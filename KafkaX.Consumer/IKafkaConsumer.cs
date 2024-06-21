using Avro.Specific;

namespace KafkaX;

public interface IKafkaConsumer
{
    Task<TValue> ConsumeXAsync<TValue>(
                   string topic,
                   CancellationToken cancellationToken = default(CancellationToken))
        where TValue : ISpecificRecord;
    void Subscribe(string topic);
}
