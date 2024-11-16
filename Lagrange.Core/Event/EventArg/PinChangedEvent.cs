namespace Lagrange.Core.Event.EventArg;

public class PinChangedEvent : EventBase
{
    public ChatType Type { get; }

    public uint Uin { get; }

    public bool IsPin { get; }

    public PinChangedEvent(ChatType type, uint uin, bool isPin)
    {
        Type = type;
        Uin = uin;
        IsPin = isPin;

        EventMessage = $"{nameof(PinChangedEvent)} {{ChatType: {Type} | Uin: {Uin} | IsPin: {IsPin}}}";
    }

    public enum ChatType
    {
        Friend,
        Group,
        Service
    }
}
