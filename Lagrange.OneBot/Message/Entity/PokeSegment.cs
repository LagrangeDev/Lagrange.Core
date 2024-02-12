using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class PokeSegment(uint type)
{
    public PokeSegment() : this(0) { }

    [JsonPropertyName("type")] [CQProperty] public uint Type { get; set; } = type;
    
    [JsonPropertyName("id")] [CQProperty] public uint Id { get; set; }
}

[SegmentSubscriber(typeof(PokeEntity), "poke")]
public partial class PokeSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is PokeSegment pokeSegment) builder.Poke(pokeSegment.Type);
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not PokeEntity pokeEntity) throw new ArgumentException("Invalid entity type.");
        
        return new PokeSegment(pokeEntity.Type);
    }
}