using System.Numerics;
using LiteDB;

namespace Lagrange.OneBot.Utility;

public static class Vector2Mapper
{
    public static void RegisterType() => BsonMapper.Global.RegisterType(Serialize, Deserialize);

    public static BsonValue Serialize(Vector2 parameter) => new BsonDocument(new Dictionary<string, BsonValue>
    {
        { "X", parameter.X },
        { "Y", parameter.Y }
    });

    public static Vector2 Deserialize(BsonValue bsonValue)
    {
        float x = (float)bsonValue["X"].AsDouble;
        float y = (float)bsonValue["Y"].AsDouble;
        return new Vector2(x, y);
    }
}