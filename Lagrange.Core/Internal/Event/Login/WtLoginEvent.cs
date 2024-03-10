#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Login;

internal class WtLoginEvent : ProtocolEvent
{
    public State EventState { get; set; }
    public byte Age { get; }
    public byte Sex { get; }
    public string Name { get; }

    public string Tag { get; } = string.Empty;
    public string Message { get; } = string.Empty;

    private WtLoginEvent(State eventState) : base(true) => EventState = eventState;

    private WtLoginEvent(int result) : base(result) { }

    private WtLoginEvent(int result, byte age, byte sex, string name) : base(result)
    {
        Age = age;
        Sex = sex;
        Name = name;
    }

    private WtLoginEvent(int result, string tag, string message) : base(result)
    {
        Tag = tag;
        Message = message;
    }

    public static WtLoginEvent Create(State eventState) => new(eventState);

    public static WtLoginEvent Result(int result) => new(result);

    public static WtLoginEvent Result(byte age, byte sex, string name)
        => new(0, age, sex, name);

    public static WtLoginEvent Result(int result, string tag, string message)
        => new(result, tag, message);

    public enum State : ushort
    {
        SubmitCaptcha = 2,
        SubmitSmsCode = 7,
        RequestSendSms = 8,
        Login = 9,
        LoginWithA2 = 9999,
    }
}