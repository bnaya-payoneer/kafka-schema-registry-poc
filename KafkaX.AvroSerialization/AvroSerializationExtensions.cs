using Avro;
using Avro.IO;
using Avro.Specific;
using Confluent.Kafka;
using System.Reflection;
using System.Text;

namespace KafkaX;

public static class AvroSerializationExtensions
{
    #region SerializeToAvro

    public static byte[] SerializeToAvro<T>(this T data)
        where T : ISpecificRecord
    {
        SerializerSchemaData<T> singleSchemaData = ExtractSchemaData<T>();
        using var output = new MemoryStream(1024);
        using var binaryWriter = new BinaryWriter(output);
        binaryWriter.Write(Encoding.UTF8.GetBytes(typeof(T).FullName));
        singleSchemaData.AvroWriter.Write(data, new BinaryEncoder(output));
        var array = output.ToArray();
        return array;
    }

    #endregion //  SerializeToAvro

    #region DeserializeFromAvro

    public static T DeserializeFromAvro<T>(this byte[] data, Schema schema)
        where T : ISpecificRecord
    {
        var start = (Encoding.UTF8.GetBytes(typeof(T).FullName)).Length;

        var subArrayLength = data.Length - start;
        var subArray = new byte[subArrayLength];
        Array.Copy(data, start, subArray, 0, subArrayLength);

        using var input = new MemoryStream(subArray);
        using var avroStream = new MemoryStream(subArray);
        var datumReader = new SpecificReader<T>(schema, schema);
        var decoder = new BinaryDecoder(avroStream);
        return datumReader.Read(default, decoder);
    }

    #endregion //  DeserializeFromAvro

    #region GetAvroSchema

    public static string GetAvroSchema(this Type type)
    {
        if (!type.IsClass || type.IsArray || type.IsAbstract || type.IsInterface)
            throw new ArgumentException("Type must be a non-abstract class.");

        var properties = type.GetAvroProperties();

        var fields = properties.Where(p => p.PropertyType != typeof(Schema))
            .Select(p => $"{{\"name\":\"{p.Name}\",\"type\":{GetAvroType(p.PropertyType)}}}");

        var schema = $"{{\"type\":\"record\",\"name\":\"{type.Name}\",\"fields\":[{string.Join(",", fields)}]}}";
        return schema;
    }

    #endregion //  GetAvroSchema

    #region GetAvroProperties

    internal static PropertyInfo[] GetAvroProperties(this Type type)
    {
        if (!type.IsClass || type.IsArray || type.IsAbstract || type.IsInterface)
            throw new ArgumentException("Type must be a non-abstract class.");

        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    #endregion //  GetAvroProperties

    #region GetAvroType

    private static string GetAvroType(Type type)
    {
        return type switch
        {
            var t when t == typeof(int) => "\"int\"",
            var t when t == typeof(long) => "\"long\"",
            var t when t == typeof(float) => "\"float\"",
            var t when t == typeof(double) => "\"double\"",
            var t when t == typeof(bool) => "\"boolean\"",
            var t when t == typeof(string) => "\"string\"",
            var t when t == typeof(byte[]) => "\"bytes\"",
            var t when t.IsEnum => $"{{\"type\":\"enum\",\"name\":\"{t.Name}\",\"symbols\":[{string.Join(",", t.GetEnumNames().Select(enumName => $"\"{enumName}\""))}]}}",
            var t when t.IsArray => $"{{\"type\":\"array\",\"items\":{GetAvroType(t.GetElementType())}}}",
            var t when t == typeof(Dictionary<string, object>) => "\"map\"",
            var t when t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>) => $"{{\"type\":\"map\",\"values\":{GetAvroType(t.GetGenericArguments()[1])}}}",
            var t when t.IsClass && t != typeof(string) => $"{{\"type\":\"record\",\"name\":\"{t.Name}\",\"fields\":{GetAvroRecordFields(t)}}}",
            _ => throw new ArgumentException($"Unsupported type: {type.Name}")
        };
    }

    #endregion //  GetAvroType

    #region GetAvroRecordFields

    private static string GetAvroRecordFields(Type type)
    {
        var properties = type.GetAvroProperties();

        var fields = properties.Where(p => p.PropertyType != typeof(Schema))
            .Select(p => $"{{\"name\":\"{p.Name}\",\"type\":{GetAvroType(p.PropertyType)}}}");

        return $"[{string.Join(",", fields)}]";
    }

    #endregion //  GetAvroRecordFields

    #region ExtractSchemaData

    private static SerializerSchemaData<T> ExtractSchemaData<T>()
    {
        System.Type writerType = typeof(T);
        // if (!(writerType != typeof (ISpecificRecord)))
        //   return;

        var schemaData = new SerializerSchemaData<T>();
        if (typeof(ISpecificRecord).IsAssignableFrom(writerType))
            schemaData.WriterSchema = ((ISpecificRecord)Activator.CreateInstance(writerType)).Schema;
        else if (writerType.Equals(typeof(int)))
            schemaData.WriterSchema = Avro.Schema.Parse("int");
        else if (writerType.Equals(typeof(bool)))
            schemaData.WriterSchema = Avro.Schema.Parse("boolean");
        else if (writerType.Equals(typeof(double)))
            schemaData.WriterSchema = Avro.Schema.Parse("double");
        else if (writerType.Equals(typeof(string)))
            schemaData.WriterSchema = Avro.Schema.Parse("string");
        else if (writerType.Equals(typeof(float)))
            schemaData.WriterSchema = Avro.Schema.Parse("float");
        else if (writerType.Equals(typeof(long)))
            schemaData.WriterSchema = Avro.Schema.Parse("long");
        else if (writerType.Equals(typeof(byte[])))
        {
            schemaData.WriterSchema = Avro.Schema.Parse("bytes");
        }
        else
        {
            if (!writerType.Equals(typeof(Null)))
                throw new InvalidOperationException("AvroSerializer only accepts type parameters of int, bool, double, string, float, long, byte[], instances of ISpecificRecord and subclasses of SpecificFixed.");
            schemaData.WriterSchema = Avro.Schema.Parse("null");
        }
        schemaData.AvroWriter = new SpecificWriter<T>(schemaData.WriterSchema);
        schemaData.WriterSchemaString = schemaData.WriterSchema.ToString();
        return schemaData;
    }

    #endregion //  ExtractSchemaData
}
