using Linq2DB_TPHProblem;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;

await using var context = new ExampleContext();
context.Database.EnsureCreated();

LinqToDBForEFTools.Initialize();
var linq2DB = context.CreateLinqToDBConnection();

// Works 
await Linq2DBMerge();

// Doesn't work
await EntityFrameworkMerge();

async Task Linq2DBMerge() // Works
{
    var tempTableName = Guid.NewGuid().ToString().Replace("-", "");

    var tempTable = await linq2DB.CreateTempTableAsync(new List<MappedEntityType1>()
    {
        new() { Id = Guid.NewGuid(), Description = "Test1", Type1EntityProp = "Prop1" },
        new() { Id = Guid.NewGuid(), Description = "Test2", Type1EntityProp = "Prop2" },
    }, new BulkCopyOptions { KeepIdentity = true }, tempTableName);
    
    var destinationTable = linq2DB.GetTable<MappedEntityType1>();
    
    destinationTable
        .Merge()
        .Using(tempTable)
        .On((target, source) => target.Id == source.Id)
        .InsertWhenNotMatched()
        .UpdateWhenMatched()
        .DeleteWhenNotMatchedBySourceAnd(i => i.Type == EntityType.Type1)
        .Merge();
}

async Task EntityFrameworkMerge() // Doesn't work
{
    var tempTableName = Guid.NewGuid().ToString().Replace("-", "");
    
    var tempTable = await linq2DB.CreateTempTableAsync(new List<Type1Entity>()
    {
        new() { Id = Guid.NewGuid(), Description = "Test1", Type1EntityProp = "Prop1" },
        new() { Id = Guid.NewGuid(), Description = "Test2", Type1EntityProp = "Prop2" },
    }, new BulkCopyOptions { KeepIdentity = true }, tempTableName);
    
    var destinationTable = linq2DB.GetTable<Type1Entity>();

    tempTable
        .Merge()
        .Using(destinationTable)
        .On((target, source) => target.Id == source.Id)
        .InsertWhenNotMatched()
        .UpdateWhenMatched()
        .DeleteWhenNotMatchedBySourceAnd(i => i.Type == EntityType.Type1)
        .Merge();
}


    
