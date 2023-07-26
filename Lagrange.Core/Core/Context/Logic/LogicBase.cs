using Lagrange.Core.Core.Event.Protocol;

namespace Lagrange.Core.Core.Context.Logic;

internal abstract class LogicBase
{
    protected readonly ContextCollection Collection;
    
    protected LogicBase(ContextCollection collection) => Collection = collection;

    public virtual Task Incoming(ProtocolEvent e) => Task.CompletedTask;
    
    public virtual Task Outgoing(ProtocolEvent e) => Task.CompletedTask;
}