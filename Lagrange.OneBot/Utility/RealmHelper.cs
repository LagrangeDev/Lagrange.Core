using Realms;

namespace Lagrange.OneBot.Utility;

public class RealmHelper(RealmConfiguration configuration)
{
    private readonly RealmConfiguration _configuration = configuration;

    public T Do<T>(Func<Realm, T> action)
    {
        using Realm realm = Realm.GetInstance(_configuration);
        return action(realm);
    }
    
    public void Do(Action<Realm> action)
    {
        using Realm realm = Realm.GetInstance(_configuration);
        action(realm);
    }
}