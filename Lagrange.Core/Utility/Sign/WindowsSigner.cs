using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Utility.Sign;

internal class WindowsSigner : UrlSigner
{
    public WindowsSigner() : base(null) { }
}