using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaX;

/// <summary>
/// Provide new schema
/// </summary>
/// <param name="key">The key.</param>
/// <param name="version">The version.</param>
/// <returns>The schema definition</returns>
public delegate Task<string> ProvideNewSchema(string key, int version);
