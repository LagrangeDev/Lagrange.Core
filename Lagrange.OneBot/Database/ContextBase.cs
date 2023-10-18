namespace Lagrange.OneBot.Database;

public abstract class ContextBase : IDisposable
{
    public abstract void Insert<T>(T value);

    public abstract int InsertRange<T>(IEnumerable<T> value, int batchSize = 5000);

    public abstract T Query<T>(Func<T, bool> predicate);
    
    public abstract void Dispose();
}