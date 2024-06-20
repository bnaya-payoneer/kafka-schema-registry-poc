using KafkaX;

namespace KafkaXWebApp;

public readonly record struct Foo(int Id, string Product, int amount);

//public readonly record struct Foo(int Id, string Product, int amount) : IKafkaIdentifier
//{
//    SchemaIdentifier IKafkaIdentifier.SchemaIdentifier { get; } = new SchemaIdentifier("foo", 0);
//}
