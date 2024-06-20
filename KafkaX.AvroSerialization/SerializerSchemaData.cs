using Avro.Specific;

namespace KafkaX;

internal class SerializerSchemaData<T>
{
    private string writerSchemaString;
    private Avro.Schema writerSchema;
    /// <remarks>
    ///     A given schema is uniquely identified by a schema id, even when
    ///     registered against multiple subjects.
    /// </remarks>
    private int? writerSchemaId;
    private SpecificWriter<T> avroWriter;
    private HashSet<string> subjectsRegistered = new HashSet<string>();

    public HashSet<string> SubjectsRegistered
    {
        get => this.subjectsRegistered;
        set => this.subjectsRegistered = value;
    }

    public string WriterSchemaString
    {
        get => this.writerSchemaString;
        set => this.writerSchemaString = value;
    }

    public Avro.Schema WriterSchema
    {
        get => this.writerSchema;
        set => this.writerSchema = value;
    }

    public int? WriterSchemaId
    {
        get => this.writerSchemaId;
        set => this.writerSchemaId = value;
    }

    public SpecificWriter<T> AvroWriter
    {
        get => this.avroWriter;
        set => this.avroWriter = value;
    }
}
