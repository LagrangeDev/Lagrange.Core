namespace Lagrange.Core.Internal.Event.Login;

internal class PasswordLoginEvent : ProtocolEvent
{
    public string? Tag { get; set; }
    
    public string? Message { get; set; }

    private PasswordLoginEvent() : base(true) { }

    private PasswordLoginEvent(int result, string? tag = null, string? message = null) : base(result)
    {
        Tag = tag;
        Message = message;
    }

    public static PasswordLoginEvent Create() => new();

    public static PasswordLoginEvent Result(int result, string? tag = null, string? message = null) => new(result, tag, message);
}