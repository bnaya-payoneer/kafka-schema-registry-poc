namespace KafkaX;

using Microsoft.EntityFrameworkCore;

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
