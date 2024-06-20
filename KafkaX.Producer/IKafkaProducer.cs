using Avro.Specific;
using Confluent.Kafka;

namespace KafkaX;
public interface IKafkaProducer
{
    Task<DeliveryResult<Null, byte[]>> ProduceXAsync<TValue>(string topic, TValue payload, int version = -1, CancellationToken cancellationToken = default) where TValue : ISpecificRecord;
}