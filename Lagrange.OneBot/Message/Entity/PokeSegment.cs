using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class PokeSegment(uint type, uint strength)
{
    public PokeSegment() : this(0, 0) { }

    [JsonPropertyName("type")] [CQProperty] public string Type { get; set; } = type.ToString();
    
    [JsonPropertyName("strength")] public string? Strength { get; set; } = strength.ToString();

    [JsonPropertyName("id")] [CQProperty] public string Id { get; set; } = "-1";  // can not get id
}

[SegmentSubscriber(typeof(PokeEntity), "poke")]
public partial class PokeSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is PokeSegment pokeSegment) builder.Poke(uint.Parse(pokeSegment.Type), pokeSegment.Strength == null ? 0 : uint.Parse(pokeSegment.Strength));
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not PokeEntity pokeEntity) throw new ArgumentException("Invalid entity type.");
        
        return new PokeSegment(pokeEntity.Type, pokeEntity.Strength);
    }
}