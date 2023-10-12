namespace Lagrange.OneBot.Core.Operation;

[AttributeUsage(AttributeTargets.Class)]
public class OperationAttribute(string api) : Attribute
{
    public string Api => api;
}