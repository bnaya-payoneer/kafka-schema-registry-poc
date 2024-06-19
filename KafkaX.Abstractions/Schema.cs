using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

