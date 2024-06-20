using KafkaX;

namespace KafkaXWebApp;

public partial class Person : SpecificAvroRecord<Person>
{
    public string Name { get; set; } = "Mona";
    public int Code { get; set; } = 5;

    public override string ToString() => $"Name: {Name}, Code: {Code}";
}

