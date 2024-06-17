namespace Lagrange.Core.Internal.Event.System;

internal class SetNeedToConfirmSwitchEvent : ProtocolEvent
{
    public bool EnableNoNeed { get; }
    

    private SetNeedToConfirmSwitchEvent(bool enableNoNeed) : base(true)
    {
        EnableNoNeed = enableNoNeed;
    }

    private SetNeedToConfirmSwitchEvent(int resultCode) : base(resultCode) { }
    
    public static SetNeedToConfirmSwitchEvent Create(bool enableNoNeed) => new(enableNoNeed);
    
    public static SetNeedToConfirmSwitchEvent Result(int resultCode) => new(resultCode);
}