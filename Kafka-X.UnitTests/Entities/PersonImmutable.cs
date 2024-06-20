using Generator.Equals;
using KafkaX;

namespace Kafka_X.UnitTests;

[Equatable]
public partial class PersonImmutable : SpecificAvroRecord<PersonCls>
{
    public string Name { get; init; } = "Mona";
    public int Code { get; init; } = 5;
}
