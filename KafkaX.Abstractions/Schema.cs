namespace KafkaX;

/// <summary>
/// The schema data entry
/// </summary>
/// <param name="Identifier">The schema identity</param>
/// <param name="Definition">The schema definition</param>
/// <param name="ModifiedDate"></param>
public readonly record struct Schema(SchemaIdentifier Identifier,
                                      byte[] Definition,
                                      DateTimeOffset ModifiedDate)
{
}

