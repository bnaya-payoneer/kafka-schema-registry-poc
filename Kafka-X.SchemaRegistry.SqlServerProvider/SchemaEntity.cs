using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KafkaX;
internal record SchemaEntity
{
    public required string Key { get; init; }
    public required int Version { get; init; }
    public required byte[] Definition { get; init; }

    public static implicit operator byte[] (SchemaEntity schemaEntity) => schemaEntity.Definition;
}
