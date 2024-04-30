using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using LinqToDB.Mapping;

namespace Linq2DB_TPHProblem;

public class ExampleContext : DbContext
{
    private readonly string _connectionString =
        "Server=localhost;Database=Linq2DB_TPHProblem;Persist Security Info=False;User ID=sa;Password=Test123456;MultipleActiveResultSets=true;Connection Timeout=60;Command Timeout=0;TrustServerCertificate=true;";

    public DbSet<BaseEntity> BaseEntities;
    public DbSet<BaseEntity> Type1Entities;
    public DbSet<BaseEntity> Type2Entities;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseEntity>(builder =>
        {
            builder.HasDiscriminator(x => x.Type)
                .HasValue<BaseEntity>(EntityType.None)
                .HasValue<Type1Entity>(EntityType.Type1)
                .HasValue<Type2Entity>(EntityType.Type2);
        });
    }
}

public enum EntityType { None, Type1, Type2 }

public class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Description { get; set; }
    public EntityType Type { get; set; }
}

public class Type1Entity : BaseEntity
{
    public string Type1EntityProp { get; set; }
}

public class Type2Entity : BaseEntity
{
    public string Type2EntityProp { get; set; }
}

