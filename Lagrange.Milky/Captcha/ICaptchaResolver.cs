using System.Threading;
using System.Threading.Tasks;

namespace Lagrange.Milky.Captcha;

public interface ICaptchaResolver
{
    Task<(string Ticket, string Randstr)> ResolveCaptchaAsync(string url, CancellationToken ct = default);
}