using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Lagrange.Milky.Http;

public interface IHttpHandler
{
    bool CanHandle(HttpListenerContext context);

    Task HandleAsync(HttpListenerContext context, CancellationToken ct);
}