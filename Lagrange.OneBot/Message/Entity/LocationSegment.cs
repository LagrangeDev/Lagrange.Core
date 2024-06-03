using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Core.Entity.Common;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class LocationSegment(float latitude, float longitude)
{
    public LocationSegment() : this(0f, 0f) { }

    [JsonPropertyName("lat")] [CQProperty] public string Latitude { get; set; } = latitude.ToString("F5");

    [JsonPropertyName("lon")] [CQProperty] public string Longitude { get; set; } = longitude.ToString("F5");

    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;

    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
}

[SegmentSubscriber(typeof(LightAppEntity), "location")]
public partial class LocationSegment : SegmentBase
{
    private static readonly JsonSerializerOptions Options = new() { Converters = { new AutosizeConverter() } };
    
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is not LocationSegment location) return;

        var json = new LightApp
        {
            App = "com.tencent.map",
            Config = new Config
            {
                Autosize = false,
                Ctime = DateTimeOffset.UtcNow.Second,
                Token = "626399d3453d0693fe19e12cd3747c56",
                Type = "normal"
            },
            Desc = "",
            From = 1,
            Meta = new Meta
            {
                LocationSearch = new LocationSearch
                {
                    EnumRelationType = 1,
                    From = "plusPanel",
                    Id = "",
                    Lat = location.Latitude,
                    Lng = location.Longitude,
                    Name = location.Title,
                    Address = location.Content
                }
            },
            Prompt = $"[Location]{location.Content}",
            Ver = "1.1.2.21",
            View = "LocationShare"
        };
        
        builder.LightApp(JsonSerializer.Serialize(json));
    }

    public override SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not LightAppEntity lightApp) throw new ArgumentException("Invalid entity type.");

        if (JsonSerializer.Deserialize<LightApp>(lightApp.Payload, Options) is { App: "com.tencent.map" } app)
        {
            return new LocationSegment
            {
                Latitude = app.Meta.LocationSearch.Lat,
                Longitude =app.Meta.LocationSearch.Lng,
                Content = app.Meta.LocationSearch.Address,
                Title = app.Meta.LocationSearch.Name,
            };
        }
        
        return null;
    }
}