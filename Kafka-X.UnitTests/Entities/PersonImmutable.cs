using AvroSchema;

namespace Kafka_X.UnitTests;

public class PersonImmutable: SpecificAvroRecord<PersonCls>
{
    public string Name { get; init; } = "Mona";
    public int Code { get; init; } = 5;
}
