using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Network;

namespace Lagrange.Core.Utility.Sign;

internal class LinuxSigner : UrlSigner
{
    public LinuxSigner() : base("https://sign.lagrangecore.org/api/sign/25765") { }
}