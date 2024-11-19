namespace Lagrange.Core.Internal.Packets.Service.WebSso;

[AttributeUsage(AttributeTargets.Class)]
internal class WebSsoAttribute : Attribute
{
    public string Host { get; set; }

    public string Cmd { get; set; }

    public WebSsoAttribute(string host, string cmd)
    {
        Host = host;
        Cmd = cmd;
    }
}