// Ignore Spelling: Avro

using KafkaX;
using Xunit.Abstractions;

namespace Kafka_X.UnitTests;

public class AvroSerializationTests
{
    private readonly ITestOutputHelper _output;

    public AvroSerializationTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void SerializationTest()
    {
        var p = new PersonCls
        {
            Name = "Sally",
            Code = 1234
        };

        Avro.Schema schema = PersonCls.SCHEMA;
        //Avro.Schema schema = Schema.Parse(typeof(PersonCls).GetAvroSchema());
        _output.WriteLine($"Schema: {schema.ToString()}");
        var buffer = schema.SerializeToAvro(p);
        _output.WriteLine($"Length: {buffer.Length}");

        var p1 = schema.DeserializeFromAvro<PersonCls>(buffer);

        Assert.Equal(p.Name, p1.Name);
        Assert.Equal(p.Code, p1.Code);
    }
}