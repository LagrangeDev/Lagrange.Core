namespace Lagrange.OneBot.Core.Operation;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class OperationAttribute(string api) : Attribute
{
    public string Api => api;
}