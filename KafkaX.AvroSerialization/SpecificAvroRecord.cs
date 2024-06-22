// Ignore Spelling: Avro

using Avro;
using Avro.Specific;
using Generator.Equals;
using System.Reflection;

namespace KafkaX;

// TODO: bnaya 2024-06-22 Source code generation for SpecificAvroRecord without reflection

public class SpecificAvroRecord<T> : ISpecificRecord // where T : new()
{
    private static readonly PropertyInfo[] PROPS = typeof(T).GetAvroProperties();
    [IgnoreEquality]
    public static readonly Schema SCHEMA = Schema.Parse(typeof(T).GetAvroSchema());

    [IgnoreEquality]
    public virtual Schema Schema => SCHEMA;

    public virtual object? Get(int fieldPos)
    {
        var propertyInfo = PROPS[fieldPos];
        return propertyInfo.GetValue(this);
    }

    public virtual void Put(int fieldPos, object fieldValue)
    {
        var propertyInfo = PROPS[fieldPos];
        propertyInfo.SetValue(this, fieldValue);
    }
}
