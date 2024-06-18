using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaX;
public readonly record struct SchemaIdentifier (
                                string schemaKey,
                                int schemaVersion);
