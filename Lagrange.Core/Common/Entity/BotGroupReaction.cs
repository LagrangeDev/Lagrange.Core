namespace Lagrange.Core.Common.Entity;

public class BotGroupReaction
{
    public string FaceId { get; set; }

    public uint Type { get; set; }

    public uint Count { get; set; }

    public bool IsAdded { get; set; }

    public BotGroupReaction(string faceId, uint type, uint count, bool isAdded)
    {
        FaceId = faceId;
        Type = type;
        Count = count;
        IsAdded = isAdded;
    }
}