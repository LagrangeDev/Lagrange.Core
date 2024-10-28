using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using LiteDB;

namespace Lagrange.OneBot.Utility;

public static class LiteDbUtility
{
    public static BsonValue IMessageEntitySerialize(IMessageEntity entity)
    {
        var type = entity.GetType();
        var result = BsonMapper.Global.Serialize(type, entity);
        result["_type"] = new BsonValue(DefaultTypeNameBinder.Instance.GetName(type));
        return result;
    }

    public static IMessageEntity IMessageEntityDeserialize(BsonValue bson)
    {
        if (!bson.IsDocument) throw new Exception("bson not BsonDocument");

        var doc = bson.AsDocument;
        if (!doc.TryGetValue("_type", out var typeBson) || !typeBson.IsString)
        {
            throw new Exception("no `_type` or `_type` not string");
        }

        var type = DefaultTypeNameBinder.Instance.GetType(typeBson.AsString);
        
        if (type == typeof(MarkdownEntity)) return MarkdownEntityDeserialize(doc);

        return (IMessageEntity)BsonMapper.Global.Deserialize(type, bson);
    }

    private static MarkdownEntity MarkdownEntityDeserialize(BsonDocument doc)
    {
        if (!doc.TryGetValue("Data", out var dataBson) || !dataBson.IsDocument)
        {
            throw new Exception("no `Data` or `Data` not document");
        }

        var dataDocument = dataBson.AsDocument;
        if (!dataDocument.TryGetValue("Content", out var contentBson) || !contentBson.IsString)
        {
            throw new InvalidCastException("no `Data.Content` or `Data.Content` not string");
        }

        return new(new MarkdownData() { Content = contentBson.AsString });
    }
}