using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Events;

namespace Lagrange.Milky.Events;

public interface IGenericEventHandler
{
    Task OnEvent<TEvent>(BotContext lagrange, TEvent @event) where TEvent : EventBase;
}