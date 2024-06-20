using Generator.Equals;
using KafkaX;

namespace Kafka_X.UnitTests;

[Equatable]
public partial class PersonCls: SpecificAvroRecord<PersonCls>
{
    public string Name { get; set; } = "Mona";
    public int Code { get; set; } = 5;
}
