using System;
using System.Collections.Generic;
using System.Reflection;
using Avro;
using Avro.Generic;
using Avro.IO;
using Avro.Specific;

public static class AvroSchemaGenerator
{
    public static string GenerateSchema<T>()
    {
        var recordSchema = CreateRecordSchema(typeof(T));
        return recordSchema.ToString();
    }

    private static RecordSchema CreateRecordSchema(Type type)
    {
        var fields = new List<Field>();

        int i = 0;
        foreach (var property in type.GetProperties())
        {
            var fieldSchema = GetSchema(property.PropertyType);
            var field = new Field(fieldSchema, property!.Name, i++, null);
            fields.Add(field);
        }

        return RecordSchema.Create(type.Name, fields, "record");
    }

    private static Schema GetSchema(Type type)
    {
        if (type == typeof(int))
            return Schema.Parse("{\"type\":\"int\"}");
        if (type == typeof(string))
            return Schema.Parse("{\"type\":\"string\"}");
        if (type == typeof(long))
            return Schema.Parse("{\"type\":\"long\"}");
        if (type == typeof(bool))
            return Schema.Parse("{\"type\":\"boolean\"}");
        if (type == typeof(float))
            return Schema.Parse("{\"type\":\"float\"}");
        if (type == typeof(double))
            return Schema.Parse("{\"type\":\"double\"}");
        if (type.IsArray)
        {
            var elementTypeSchema = GetSchema(type.GetElementType());
            return Schema.Parse("{\"type\":\"array\", \"items\":" + elementTypeSchema.ToString() + "}");
        }
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            var itemType = type.GetGenericArguments()[0];
            var itemSchema = GetSchema(itemType);
            return Schema.Parse("{\"type\":\"array\", \"items\":" + itemSchema.ToString() + "}");
        }
        if (type.IsClass)
            return CreateRecordSchema(type);

        throw new ArgumentException($"Unsupported type: {type.FullName}");
    }

    public static byte[] SerializeToAvro<T>(this T obj, Schema schema)
    {
        var record = new GenericRecord((RecordSchema)schema);

        foreach (var field in schema.Fields)
        {
            var property = typeof(T).GetProperty(field.Name, BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                record.Add(field.Name, property.GetValue(obj));
            }
        }

        using (var memoryStream = new MemoryStream())
        {
            var writer = new SpecificDefaultWriter(schema);
            var encoder = new BinaryEncoder(memoryStream);
            writer.Write(record, encoder);
            return memoryStream.ToArray();
        }
    }
}
