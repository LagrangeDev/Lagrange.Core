namespace Lagrange.Core.Message;

public class MessageResult
{
    public ulong MessageId { get; internal set; } 
    
    public uint? Sequence { get; internal set; }
    
    public uint Result { get; internal set; }
    
    public uint Timestamp { get; internal set; }
    
    public uint ClientSequence { get; internal set; }
}