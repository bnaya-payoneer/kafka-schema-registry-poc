using Riok.Mapperly.Abstractions;

namespace KafkaXWebApp;

[Mapper]
public static partial class PersonMapper
{
    public static partial Person FromEntity(this PersonEntity person);
}

