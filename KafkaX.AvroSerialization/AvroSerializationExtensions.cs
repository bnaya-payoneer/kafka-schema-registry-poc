using Avro.IO;
using Avro.Specific;
using Confluent.Kafka;
using System.Net;
using System.Text;
using Encoder = Avro.IO.Encoder;

namespace KafkaX;

public static class AvroSerializationExtensions
{
    private static SerializerSchemaData<T> ExtractSchemaData<T>(T data)
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

    public static byte[] SerializeToAvro<T>(this T data)
        where T : ISpecificRecord
    {
        string type = data.GetType().FullName!;
        byte[] key = Encoding.UTF8.GetBytes(type);
        var singleSchemaData = ExtractSchemaData(data);
        using (MemoryStream output = new MemoryStream(1024))
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(output))
            {
                output.WriteByte((byte)0);
                binaryWriter.Write(key);
                var encoder = (Encoder)new BinaryEncoder(output);
                singleSchemaData.AvroWriter.Write(data, encoder);
                var array = output.ToArray();
                return array;
            }
        }
    }
}
