using Lagrange.Proto.Serialization;

namespace Lagrange.Proto.Test;

/// <summary>
/// Regression tests for https://github.com/LagrangeDev/Lagrange.Core/issues/917
/// Negative values of signed integer types with default (unsigned varint) handling
/// corrupt the serialized payload.
/// </summary>
[TestFixture]
public class NegativeVarIntTest
{
    [Test]
    public void TestNegativeInt_Roundtrip_Reflection()
    {
        var obj = new NegativeVarIntMessage
        {
            IntField = default,
            StringField1 = null,
            StringField2 = null,
            NegativeIntField = -1
        };

        byte[] bytes = ProtoSerializer.Serialize(obj);
        var deserialized = ProtoSerializer.Deserialize<NegativeVarIntMessage>(bytes);

        Assert.Multiple(() =>
        {
            Assert.That(deserialized.IntField, Is.EqualTo(obj.IntField));
            Assert.That(deserialized.StringField1, Is.EqualTo(obj.StringField1));
            Assert.That(deserialized.StringField2, Is.EqualTo(obj.StringField2));
            Assert.That(deserialized.NegativeIntField, Is.EqualTo(obj.NegativeIntField));
        });
    }

    [Test]
    public void TestNegativeInt_Roundtrip_SourceGenerated()
    {
        var obj = new NegativeVarIntMessage
        {
            IntField = default,
            StringField1 = null,
            StringField2 = null,
            NegativeIntField = -1
        };

        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        var deserialized = ProtoSerializer.DeserializeProtoPackable<NegativeVarIntMessage>(bytes);

        Assert.Multiple(() =>
        {
            Assert.That(deserialized.IntField, Is.EqualTo(obj.IntField));
            Assert.That(deserialized.StringField1, Is.EqualTo(obj.StringField1));
            Assert.That(deserialized.StringField2, Is.EqualTo(obj.StringField2));
            Assert.That(deserialized.NegativeIntField, Is.EqualTo(obj.NegativeIntField));
        });
    }

    [Test]
    public void TestNegativeInt_NestedObject_Roundtrip()
    {
        var obj = new NegativeVarIntNested
        {
            Inner = new NegativeVarIntMessage { IntField = 1, StringField1 = "t", StringField2 = "d", NegativeIntField = -1 }
        };

        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        var deserialized = ProtoSerializer.DeserializeProtoPackable<NegativeVarIntNested>(bytes);

        Assert.Multiple(() =>
        {
            Assert.That(deserialized.Inner, Is.Not.Null);
            Assert.That(deserialized.Inner!.IntField, Is.EqualTo(obj.Inner.IntField));
            Assert.That(deserialized.Inner.NegativeIntField, Is.EqualTo(obj.Inner.NegativeIntField));
        });
    }

    [Test]
    public void TestNegativeValues_Boundary()
    {
        var obj = new NegativeVarIntBoundary
        {
            SByteValue = -1,
            ShortValue = -1,
            IntValue = -1,
            LongValue = -1,
            SByteMin = sbyte.MinValue,
            ShortMin = short.MinValue,
            IntMin = int.MinValue,
            LongMin = long.MinValue
        };

        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        var deserialized = ProtoSerializer.DeserializeProtoPackable<NegativeVarIntBoundary>(bytes);

        Assert.Multiple(() =>
        {
            Assert.That(deserialized.SByteValue, Is.EqualTo(obj.SByteValue));
            Assert.That(deserialized.ShortValue, Is.EqualTo(obj.ShortValue));
            Assert.That(deserialized.IntValue, Is.EqualTo(obj.IntValue));
            Assert.That(deserialized.LongValue, Is.EqualTo(obj.LongValue));
            Assert.That(deserialized.SByteMin, Is.EqualTo(obj.SByteMin));
            Assert.That(deserialized.ShortMin, Is.EqualTo(obj.ShortMin));
            Assert.That(deserialized.IntMin, Is.EqualTo(obj.IntMin));
            Assert.That(deserialized.LongMin, Is.EqualTo(obj.LongMin));
        });
    }
}

[ProtoPackable]
public partial class NegativeVarIntMessage
{
    [ProtoMember(1)] public int IntField { get; set; }
    [ProtoMember(2)] public string? StringField1 { get; set; }
    [ProtoMember(3)] public string? StringField2 { get; set; }
    [ProtoMember(4)] public int NegativeIntField { get; set; }
}

[ProtoPackable]
public partial class NegativeVarIntNested
{
    [ProtoMember(1)] public NegativeVarIntMessage? Inner { get; set; }
}

[ProtoPackable]
public partial class NegativeVarIntBoundary
{
    [ProtoMember(1)] public sbyte SByteValue { get; set; }
    [ProtoMember(2)] public short ShortValue { get; set; }
    [ProtoMember(3)] public int IntValue { get; set; }
    [ProtoMember(4)] public long LongValue { get; set; }
    [ProtoMember(5)] public sbyte SByteMin { get; set; }
    [ProtoMember(6)] public short ShortMin { get; set; }
    [ProtoMember(7)] public int IntMin { get; set; }
    [ProtoMember(8)] public long LongMin { get; set; }
}
