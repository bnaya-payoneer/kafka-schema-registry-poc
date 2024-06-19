using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaX;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

internal class SchemaRegistryContext : DbContext
{
    public SchemaRegistryContext(DbContextOptions<SchemaRegistryContext> options) : base(options) { }

    public DbSet<SchemaEntity> SchemaCollection { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Specify the table name for SchemaEntity
        modelBuilder.Entity<SchemaEntity>()
            .ToTable("schema_registry")
            .HasKey(s => new { s.Key, s.Version });

        base.OnModelCreating(modelBuilder);
    }
}
