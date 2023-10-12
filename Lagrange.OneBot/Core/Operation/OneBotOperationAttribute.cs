namespace Lagrange.OneBot.Core.Operation;

[AttributeUsage(AttributeTargets.Class)]
public class OneBotOperationAttribute(string api)
{
    public string Api => api;
}