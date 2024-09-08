namespace Lagrange.Core.Message;

public class MessageResult
{
    public uint? Sequence { get; set; }
    
    public uint Result { get; set; }
    
    public uint Timestamp { get; set; }
    
    public uint ClientSequence { get; set; }
}