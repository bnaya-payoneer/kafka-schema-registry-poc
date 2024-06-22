namespace KafkaX;
internal record SchemaEntity
{
    public required string Key { get; init; }
    public required int Version { get; init; }
    public required string Definition { get; init; }
    public DateTimeOffset? ModifiedDate { get; init; }

    public static implicit operator string(SchemaEntity schemaEntity) => schemaEntity.Definition;
}
