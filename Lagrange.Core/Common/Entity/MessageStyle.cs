namespace Lagrange.Core.Common.Entity;

public class MessageStyle
{
    /// <summary>
    /// 气泡id
    /// </summary>
    public uint BubbleId { get; set; }

    /// <summary>
    /// 挂件id
    /// </summary>
    public long PendantId { get; set; }

    /// <summary>
    /// 字体id
    /// </summary>
    public ushort FontId { get; set; }

    /// <summary>
    /// 字体特效id，超大字为2000
    /// </summary>
    public uint FontEffectId { get; set; }

    /// <summary>
    /// 是否启用大小字特效
    /// </summary>
    public bool IsCsFontEffectEnabled { get; set; }

    /// <summary>
    /// 自定义气泡数据存档id
    /// </summary>
    public uint BubbleDiyTextId { get; set; }
}
