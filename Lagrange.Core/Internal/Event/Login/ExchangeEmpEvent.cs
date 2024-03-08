#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Login;

internal class ExchangeEmpEvent : ProtocolEvent
{
    public State EventState { get; set; }
    public byte Age { get; }
    public byte Sex { get; }
    public string Name { get; }

    public string Tag { get; } = string.Empty;
    public string Message { get; } = string.Empty;

    private ExchangeEmpEvent(State eventState) : base(true) => EventState = eventState;

    private ExchangeEmpEvent(int result) : base(result) { }

    private ExchangeEmpEvent(int result, byte age, byte sex, string name) : base(result)
    {
        Age = age;
        Sex = sex;
        Name = name;
    }

    private ExchangeEmpEvent(int result, string tag, string message) : base(result)
    {
        Tag = tag;
        Message = message;
    }

    public static ExchangeEmpEvent Create(State eventState) => new(eventState);
    
    public static ExchangeEmpEvent Result(int result) => new(result);

    public static ExchangeEmpEvent Result(byte age, byte sex, string name)
        => new(0, age, sex, name);

    public static ExchangeEmpEvent Result(int result, string tag, string message) => new(result, tag, message);

    public enum State : ushort
    {
        RefreshToken = 0x0F
    }
}