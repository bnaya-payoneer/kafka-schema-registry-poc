namespace KafkaX;
internal record SchemaEntity
{
    public required string Key { get; init; }
    public required int Version { get; init; }
    public required byte[] Definition { get; init; }
    public DateTimeOffset? ModifiedDate { get; init; }

    public static implicit operator byte[](SchemaEntity schemaEntity) => schemaEntity.Definition;
}
