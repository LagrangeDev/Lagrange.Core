#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Login;

internal class LoginEvent : ProtocolEvent
{
    public byte Age { get; }
    public byte Sex { get; }
    public string Name { get; }
    public string Tag { get; } = string.Empty;
    
    public string Message { get; } = string.Empty;
    
    private LoginEvent() : base(true) { }
    
    private LoginEvent(int resultCode) : base(resultCode) { }

    private LoginEvent(int resultCode, byte age, byte sex, string name) : base(resultCode)
    {
        Age = age;
        Sex = sex;
        Name = name;
    }
    
    private LoginEvent(int resultCode, string tag, string message) : base(resultCode)
    {
        Tag = tag;
        Message = message;
    }
    
    public static LoginEvent Create() => new();

    public static LoginEvent Result(int resultCode, byte age, byte sex, string name)
        => new(resultCode, age, sex, name);
    
    public static LoginEvent Result(int resultCode) => new(resultCode);

    
    public static LoginEvent Result(int resultCode, string tag, string message) => new(resultCode, tag, message);
}