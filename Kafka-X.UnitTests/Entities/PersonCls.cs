using AvroSchema;

namespace Kafka_X.UnitTests;

public class PersonCls : SpecificAvroRecord<PersonCls>
{
    public string Name { get; set; } = "Mona";
    public int Code { get; set; } = 5;
}
