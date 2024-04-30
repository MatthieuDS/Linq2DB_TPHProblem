using LinqToDB.Mapping;

namespace Linq2DB_TPHProblem;

[Table("BaseEntity")]
public class MappedEntityType1
{
    [PrimaryKey]
    public Guid Id { get; set; }
    
    [Column]
    public string Description { get; set; }
    
    [Column]
    public string Type1EntityProp { get; set; }
    
    [Column]
    public EntityType Type { get; set; }
}