using LiteDB;
using Microsoft.Extensions.Configuration;

namespace Lagrange.OneBot.Database;

public class LiteDbContext : ContextBase
{
    private readonly LiteDatabase _instance;
    
    public LiteDbContext(IConfiguration config)
    {
        string databasePath = config["ConfigPath:Database"] ?? $"lagrange-{config["Account:Uin"]}.db";

        _instance = new LiteDatabase(databasePath);
    }

    public override void Insert<T>(ulong id, T value)
    {
        var collection = _instance.GetCollection<T>();
        collection.Insert(value);
        
        _instance.Commit();
    }

    public override int InsertRange<T>(IEnumerable<T> value, int batchSize = 5000)
    {
        var collection = _instance.GetCollection<T>();
        int count = collection.InsertBulk(value, batchSize);
        _instance.Commit();

        return count;
    }

    public override T Query<T>(Func<T, bool> predicate)
    {
        var collection = _instance.GetCollection<T>();
        return collection.FindOne(x => predicate(x));
    }

    public override void Dispose()
    {
        _instance.Dispose();
    }
}