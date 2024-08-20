namespace Lagrange.Core.Utility;

/// <summary>
/// A class that can inject services into other classes.
/// </summary>
internal sealed class ServiceInjector
{
    private readonly Dictionary<Type, object> _services = new();

    public bool AddService<T>(T service)
    {
        if (service == null) return false;

        _services.Add(typeof(T), service);
        return true;
    }

    public bool RemoveService<T>() => _services.Remove(typeof(T));

    public T GetService<T>() => (T)_services[typeof(T)];

    public bool TryGetService<T>(out T? service)
    {
        if (_services.TryGetValue(typeof(T), out object? obj))
        {
            service = (T)obj;
            return true;
        }

        service = default;
        return false;
    }

    public bool ContainsService<T>() => _services.ContainsKey(typeof(T));

    public void Clear() => _services.Clear();

    public object this[Type type]
    {
        get => _services[type];
        set => _services[type] = value;
    }

    /// <summary>
    /// Injects a service into the specified object.
    /// </summary>
    /// <param name="parameters">The parameters exclude the service that should be injected</param>
    /// <typeparam name="TInject">The class that should be injected to have new instance</typeparam>
    /// <returns></returns>
    public TInject Inject<TInject>(object?[]? parameters = null)
    {
        var constructors = typeof(TInject).GetConstructors();
        if (constructors.Length != 1) throw new InvalidOperationException("The class must have only one constructor");

        var constructor = constructors[0];
        var parametersInfo = constructor.GetParameters();
        var constructed = new object?[parametersInfo.Length];
        for (int i = 0; i < parametersInfo.Length; i++)
        {
            var parameter = parametersInfo[i];

            if (_services.TryGetValue(parameter.ParameterType, out var service))
            {
                constructed[i] = service;
            }
            else
            {
                if (parameters == null) constructed[i] = null;
                else constructed[i] = parameters[i];
            }
        }

        return (TInject)constructor.Invoke(constructed);
    }

    public object Inject(Type type, object?[]? parameters = null)
    {
        var constructors = type.GetConstructors();
        if (constructors.Length != 1) throw new InvalidOperationException("The class must have only one constructor");

        var constructor = constructors[0];
        var parametersInfo = constructor.GetParameters();
        var constructed = new object?[parametersInfo.Length];
        for (int i = 0; i < parametersInfo.Length; i++)
        {
            var parameter = parametersInfo[i];

            if (_services.TryGetValue(parameter.ParameterType, out var service))
            {
                constructed[i] = service;
            }
            else
            {
                if (parameters == null) constructed[i] = null;
                else constructed[i] = parameters[i];
            }
        }

        return constructor.Invoke(constructed);
    }
}