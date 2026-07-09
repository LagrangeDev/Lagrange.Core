using System.Text;
using Lagrange.Proto.Nodes;
using Lagrange.Proto.Primitives;
using Lagrange.Proto.Serialization;
using Lagrange.Proto.Utility;

namespace Lagrange.Proto.Test;

[TestFixture]
public class ProtoNodeTest
{
    #region ProtoValue Tests
    
    [Test]
    public void TestProtoValue_BasicTypes()
    {
        var intValue = new ProtoValue<int>(42, WireType.VarInt);
        var stringValue = new ProtoValue<string>("Hello", WireType.LengthDelimited);
        var floatValue = new ProtoValue<float>(3.14f, WireType.Fixed32);
        var doubleValue = new ProtoValue<double>(2.71828, WireType.Fixed64);
        var boolValue = new ProtoValue<bool>(true, WireType.VarInt);
        
        Assert.Multiple(() =>
        {
            Assert.That(intValue.GetValue<int>(), Is.EqualTo(42));
            Assert.That(stringValue.GetValue<string>(), Is.EqualTo("Hello"));
            Assert.That(floatValue.GetValue<float>(), Is.EqualTo(3.14f).Within(0.001f));
            Assert.That(doubleValue.GetValue<double>(), Is.EqualTo(2.71828).Within(0.00001));
            Assert.That(boolValue.GetValue<bool>(), Is.True);
        });
    }
    
    [Test]
    public void TestProtoValue_TryGetValue()
    {
        var intValue = new ProtoValue<int>(42, WireType.VarInt);
        
        bool success1 = intValue.TryGetValue<int>(out int result1);
        bool success2 = intValue.TryGetValue<string>(out string? result2);
        bool success3 = intValue.TryGetValue<long>(out long result3);
        
        Assert.Multiple(() =>
        {
            Assert.That(success1, Is.True);
            Assert.That(result1, Is.EqualTo(42));
            Assert.That(success2, Is.False);
            Assert.That(result2, Is.Null);
            Assert.That(success3, Is.True);
            Assert.That(result3, Is.EqualTo(42L));
        });
    }
    
    [Test]
    public void TestProtoValue_RawValue()
    {
        var rawValue = new ProtoRawValue(WireType.VarInt, 123456);
        var protoValue = new ProtoValue<ProtoRawValue>(rawValue, WireType.VarInt);
        
        Assert.Multiple(() =>
        {
            Assert.That(protoValue.GetValue<int>(), Is.EqualTo(123456));
            Assert.That(protoValue.GetValue<long>(), Is.EqualTo(123456L));
            Assert.That(protoValue.GetValue<uint>(), Is.EqualTo(123456U));
        });
    }
    
    [Test]
    public void TestProtoValue_BytesAsRawValue()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("Test String");
        var rawValue = new ProtoRawValue(WireType.LengthDelimited, 0) { Bytes = bytes };
        var protoValue = new ProtoValue<ProtoRawValue>(rawValue, WireType.LengthDelimited);
        
        Assert.Multiple(() =>
        {
            Assert.That(protoValue.GetValue<string>(), Is.EqualTo("Test String"));
            Assert.That(protoValue.GetValue<byte[]>(), Is.EqualTo(bytes));
        });
    }
    
    [Test]
    public void TestProtoValue_Serialization()
    {
        var value = new ProtoValue<int>(42, WireType.VarInt);
        var bufferWriter = new SegmentBufferWriter(256);
        var writer = new ProtoWriter(bufferWriter);
        
        // Write the tag first (field number 1, WireType.VarInt)
        writer.EncodeVarInt((1 << 3) | (byte)WireType.VarInt);
        value.WriteTo(1, writer);
        writer.Flush();
        
        var bytes = bufferWriter.ToArray();
        var reader = new ProtoReader(bytes);
        
        int tag = reader.DecodeVarInt<int>();
        int fieldNumber = tag >> 3;
        WireType wireType = (WireType)(tag & 0x07);
        int decodedValue = reader.DecodeVarInt<int>();
        
        Assert.Multiple(() =>
        {
            Assert.That(fieldNumber, Is.EqualTo(1));
            Assert.That(wireType, Is.EqualTo(WireType.VarInt));
            Assert.That(decodedValue, Is.EqualTo(42));
        });
        
        bufferWriter.Dispose();
    }

    [Test]
    public void TestProtoReader_DecodeVarIntUInt32_FiveByteValue()
    {
        byte[] bytes = [0xBB, 0x8C, 0x85, 0xBF, 0x06, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
        var reader = new ProtoReader(bytes);

        uint decodedValue = reader.DecodeVarInt<uint>();

        Assert.That(decodedValue, Is.EqualTo(1742816827u));
    }
    
    #endregion
    
    #region ProtoArray Tests
    
    [Test]
    public void TestProtoArray_Creation()
    {
        var array1 = new ProtoArray(WireType.VarInt, 1, 2, 3, 4, 5);
        var array2 = new ProtoArray(WireType.LengthDelimited, "Hello", "World");
        var array3 = new ProtoArray(WireType.Fixed32, 1.1f, 2.2f, 3.3f);
        
        Assert.Multiple(() =>
        {
            Assert.That(array1.Count, Is.EqualTo(5));
            Assert.That(array2.Count, Is.EqualTo(2));
            Assert.That(array3.Count, Is.EqualTo(3));
        });
    }
    
    [Test]
    public void TestProtoArray_GetValues()
    {
        var array = new ProtoArray(WireType.VarInt, 10, 20, 30, 40, 50);
        
        int[] values = array.GetValues<int>().ToArray();
        
        Assert.That(values, Is.EqualTo(new[] { 10, 20, 30, 40, 50 }));
    }
    
    [Test]
    public void TestProtoArray_Indexer()
    {
        var array = new ProtoArray(WireType.LengthDelimited, "First", "Second", "Third");
        
        Assert.Multiple(() =>
        {
            Assert.That(array[0].GetValue<string>(), Is.EqualTo("First"));
            Assert.That(array[1].GetValue<string>(), Is.EqualTo("Second"));
            Assert.That(array[2].GetValue<string>(), Is.EqualTo("Third"));
        });
    }
    
    [Test]
    public void TestProtoArray_Add()
    {
        var array = new ProtoArray(WireType.VarInt);
        
        array.Add(10);
        array.Add(20);
        array.Add(30);
        
        Assert.Multiple(() =>
        {
            Assert.That(array.Count, Is.EqualTo(3));
            Assert.That(array.GetValues<int>().ToArray(), Is.EqualTo(new[] { 10, 20, 30 }));
        });
    }
    
    [Test]
    public void TestProtoArray_Clear()
    {
        var array = new ProtoArray(WireType.VarInt, 1, 2, 3);
        
        Assert.That(array.Count, Is.EqualTo(3));
        
        array.Clear();
        
        Assert.That(array.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void TestProtoArray_Contains()
    {
        var array = new ProtoArray(WireType.VarInt, 10, 20, 30);
        
        var value20 = new ProtoValue<int>(20, WireType.VarInt);
        var value40 = new ProtoValue<int>(40, WireType.VarInt);
        
        Assert.Multiple(() =>
        {
            Assert.That(array.Any(v => v.GetValue<int>() == 20), Is.True);
            Assert.That(array.Any(v => v.GetValue<int>() == 40), Is.False);
        });
    }
    
    [Test]
    public void TestProtoArray_EmptyArray()
    {
        var array = new ProtoArray(WireType.VarInt);
        
        Assert.Multiple(() =>
        {
            Assert.That(array.Count, Is.EqualTo(0));
            Assert.That(array.GetValues<int>().ToArray(), Is.Empty);
        });
    }
    
    [Test]
    public void TestProtoArray_MixedTypes()
    {
        // ProtoArray with same type values
        var array = new ProtoArray(WireType.VarInt, 1, 2, 3);
        
        // Values should be retrievable as int
        int[] intValues = array.GetValues<int>().ToArray();
        
        Assert.That(intValues, Is.EqualTo(new int[] { 1, 2, 3 }));
    }
    
    #endregion
    
    #region ProtoObject Tests
    
    [Test]
    public void TestProtoObject_Creation()
    {
        var obj = new ProtoObject
        {
            { 1, 42 },
            { 2, "Hello" },
            { 3, 3.14f }
        };
        
        Assert.Multiple(() =>
        {
            Assert.That(obj[1].GetValue<int>(), Is.EqualTo(42));
            Assert.That(obj[2].GetValue<string>(), Is.EqualTo("Hello"));
            Assert.That(obj[3].GetValue<float>(), Is.EqualTo(3.14f).Within(0.001f));
        });
    }
    
    [Test]
    public void TestProtoObject_NestedObject()
    {
        var nested = new ProtoObject
        {
            { 1, "Nested Value" },
            { 2, 100 }
        };
        
        var obj = new ProtoObject
        {
            { 1, "Root Value" },
            { 2, nested }
        };
        
        Assert.Multiple(() =>
        {
            Assert.That(obj[1].GetValue<string>(), Is.EqualTo("Root Value"));
            Assert.That(obj[2][1].GetValue<string>(), Is.EqualTo("Nested Value"));
            Assert.That(obj[2][2].GetValue<int>(), Is.EqualTo(100));
        });
    }
    
    [Test]
    public void TestProtoObject_RepeatedFields()
    {
        var obj = new ProtoObject
        {
            { 1, 10 },
            { 1, 20 },
            { 1, 30 },
            { 2, "Single" }
        };
        
        var array = obj[1].AsArray();
        
        Assert.Multiple(() =>
        {
            Assert.That(array.Count, Is.EqualTo(3));
            Assert.That(array.GetValues<int>().ToArray(), Is.EqualTo(new[] { 10, 20, 30 }));
            Assert.That(obj[2].GetValue<string>(), Is.EqualTo("Single"));
        });
    }
    
    [Test]
    public void TestProtoObject_Parse()
    {
        var original = new ProtoObject
        {
            { 1, 42 },
            { 2, "Test" },
            { 3, new ProtoObject { { 1, 100 } } }
        };
        
        byte[] bytes = original.Serialize();
        ProtoObject parsed = ProtoObject.Parse(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(parsed[1].GetValue<int>(), Is.EqualTo(42));
            Assert.That(parsed[2].GetValue<string>(), Is.EqualTo("Test"));
            Assert.That(parsed[3][1].GetValue<int>(), Is.EqualTo(100));
        });
    }
    
    [Test]
    public void TestProtoObject_SerializeDeserialize()
    {
        var obj = new ProtoObject
        {
            { 1, 123 },
            { 2, "Hello World" },
            { 3, 45.67 },
            { 4, new ProtoArray(WireType.VarInt, 1, 2, 3) }
        };
        
        byte[] serialized = obj.Serialize();
        ProtoObject deserialized = ProtoObject.Parse(serialized);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized[1].GetValue<int>(), Is.EqualTo(123));
            Assert.That(deserialized[2].GetValue<string>(), Is.EqualTo("Hello World"));
            Assert.That(deserialized[3].GetValue<double>(), Is.EqualTo(45.67).Within(0.001));
            Assert.That(deserialized[4].AsArray().GetValues<int>().ToArray(), Is.EqualTo(new[] { 1, 2, 3 }));
        });
    }
    
    [Test]
    public void TestProtoObject_EmptyObject()
    {
        var obj = new ProtoObject();
        
        byte[] bytes = obj.Serialize();
        
        Assert.That(bytes, Is.Empty);
    }
    
    [Test]
    public void TestProtoObject_TryGetValue()
    {
        var obj = new ProtoObject
        {
            { 1, 42 },
            { 2, "Test" }
        };
        
        bool hasField1 = obj.TryGetValue(1, out ProtoNode? node1);
        bool hasField3 = obj.TryGetValue(3, out ProtoNode? node3);
        
        Assert.Multiple(() =>
        {
            Assert.That(hasField1, Is.True);
            Assert.That(node1?.GetValue<int>(), Is.EqualTo(42));
            Assert.That(hasField3, Is.False);
            Assert.That(node3, Is.Null);
        });
    }
    
    [Test]
    public void TestProtoObject_ComplexNesting()
    {
        var level3 = new ProtoObject { { 1, "Level 3" } };
        var level2 = new ProtoObject { { 1, "Level 2" }, { 2, level3 } };
        var level1 = new ProtoObject { { 1, "Level 1" }, { 2, level2 } };
        
        Assert.Multiple(() =>
        {
            Assert.That(level1[1].GetValue<string>(), Is.EqualTo("Level 1"));
            Assert.That(level1[2][1].GetValue<string>(), Is.EqualTo("Level 2"));
            Assert.That(level1[2][2][1].GetValue<string>(), Is.EqualTo("Level 3"));
        });
    }
    
    #endregion
    
    #region ProtoNode Operators Tests
    
    [Test]
    public void TestProtoNode_ImplicitOperators_Numbers()
    {
        var obj = new ProtoObject
        {
            { 1, true },
            { 2, (byte)255 },
            { 3, (byte)128 }, // Use positive values for unsigned encoding
            { 4, (ushort)32768 },
            { 5, (ushort)65535 },
            { 6, 1000 },
            { 7, (uint)4294967295 },
            { 8, 9223372036854775807L },
            { 9, (ulong)18446744073709551615 },
            { 10, 3.14f },
            { 11, 2.71828 }
        };
        
        byte[] bytes = obj.Serialize();
        ProtoObject parsed = ProtoObject.Parse(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That((bool)parsed[1], Is.True);
            Assert.That((byte)parsed[2], Is.EqualTo(255));
            Assert.That((byte)parsed[3], Is.EqualTo(128));
            Assert.That((ushort)parsed[4], Is.EqualTo(32768));
            Assert.That((ushort)parsed[5], Is.EqualTo(65535));
            Assert.That((int)parsed[6], Is.EqualTo(1000));
            Assert.That((uint)parsed[7], Is.EqualTo(4294967295));
            Assert.That((long)parsed[8], Is.EqualTo(9223372036854775807L));
            Assert.That((ulong)parsed[9], Is.EqualTo(18446744073709551615));
            Assert.That((float)parsed[10], Is.EqualTo(3.14f).Within(0.001f));
            Assert.That((double)parsed[11], Is.EqualTo(2.71828).Within(0.00001));
        });
    }
    
    [Test]
    public void TestProtoNode_ImplicitOperators_Nullable()
    {
        var obj = new ProtoObject
        {
            { 1, (int?)42 },
            { 2, (string?)null },
            { 3, (bool?)true },
            { 4, (double?)3.14159 }
        };
        
        byte[] bytes = obj.Serialize();
        ProtoObject parsed = ProtoObject.Parse(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(parsed[1].GetValue<int>(), Is.EqualTo(42));
            Assert.That(parsed.TryGetValue(2, out _), Is.False);
            Assert.That(parsed[3].GetValue<bool>(), Is.True);
            Assert.That(parsed[4].GetValue<double>(), Is.EqualTo(3.14159).Within(0.00001));
        });
    }
    
    [Test]
    public void TestProtoNode_ImplicitOperators_StringAndBytes()
    {
        var obj = new ProtoObject
        {
            { 1, "Test String" },
            { 2, "Test String".AsMemory() },
            { 3, new byte[] { 1, 2, 3, 4, 5 } },
            { 4, (ReadOnlyMemory<byte>)new byte[] { 6, 7, 8 }.AsMemory() }
        };
        
        byte[] bytes = obj.Serialize();
        ProtoObject parsed = ProtoObject.Parse(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That((string)parsed[1], Is.EqualTo("Test String"));
            Assert.That((string)parsed[2], Is.EqualTo("Test String"));
            Assert.That((byte[])parsed[3], Is.EqualTo(new byte[] { 1, 2, 3, 4, 5 }));
            Assert.That(parsed[4].GetValue<byte[]>(), Is.EqualTo(new byte[] { 6, 7, 8 }));
        });
    }
    
    #endregion
    
    #region Edge Cases
    
    [Test]
    public void TestProtoNode_LargeFieldNumbers()
    {
        var obj = new ProtoObject
        {
            { 1, "First" },
            { 100, "Middle" },
            { 536870911, "Max Field Number" } // 2^29 - 1 is max field number
        };
        
        byte[] bytes = obj.Serialize();
        ProtoObject parsed = ProtoObject.Parse(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(parsed[1].GetValue<string>(), Is.EqualTo("First"));
            Assert.That(parsed[100].GetValue<string>(), Is.EqualTo("Middle"));
            Assert.That(parsed[536870911].GetValue<string>(), Is.EqualTo("Max Field Number"));
        });
    }
    
    [Test]
    public void TestProtoArray_LargeArray()
    {
        int[] values = Enumerable.Range(0, 10000).ToArray();
        var array = new ProtoArray(WireType.VarInt);
        foreach (int value in values)
        {
            array.Add(value);
        }
        
        int[] retrieved = array.GetValues<int>().ToArray();
        
        Assert.That(retrieved, Is.EqualTo(values));
    }
    
    [Test]
    public void TestProtoObject_EmptyNestedStructures()
    {
        var obj = new ProtoObject
        {
            { 1, new ProtoObject() },
            { 2, new ProtoArray(WireType.VarInt) },
            { 3, "Not Empty" }
        };
        
        byte[] bytes = obj.Serialize();
        ProtoObject parsed = ProtoObject.Parse(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(parsed[1].AsObject(), Is.Not.Null);
            Assert.That(parsed[3].GetValue<string>(), Is.EqualTo("Not Empty"));
        });
    }
    
    [Test]
    public void TestProtoValue_ConversionEdgeCases()
    {
        var value = new ProtoValue<int>(int.MaxValue, WireType.VarInt);
        
        Assert.Multiple(() =>
        {
            Assert.That(value.GetValue<int>(), Is.EqualTo(int.MaxValue));
            Assert.That(value.GetValue<long>(), Is.EqualTo((long)int.MaxValue));
            Assert.That(value.GetValue<uint>(), Is.EqualTo((uint)int.MaxValue));
        });
    }
    
    #endregion
    
    #region Performance and Memory Tests
    
    [Test]
    public void TestProtoObject_SerializeToWriter()
    {
        var obj = new ProtoObject
        {
            { 1, 42 },
            { 2, "Test" },
            { 3, new ProtoObject { { 1, 100 } } }
        };
        
        var bufferWriter = new SegmentBufferWriter(1024);
        obj.Serialize(bufferWriter);
        
        byte[] writerBytes = bufferWriter.ToArray();
        byte[] normalBytes = obj.Serialize();
        
        Assert.That(writerBytes, Is.EqualTo(normalBytes));
        
        bufferWriter.Dispose();
    }
    
    [Test]
    public void TestProtoNode_MeasureSize()
    {
        var intValue = new ProtoValue<int>(42, WireType.VarInt);
        var stringValue = new ProtoValue<string>("Hello", WireType.LengthDelimited);
        var floatValue = new ProtoValue<float>(3.14f, WireType.Fixed32);
        
        int intSize = intValue.Measure(1);
        int stringSize = stringValue.Measure(2);
        int floatSize = floatValue.Measure(3);
        
        Assert.Multiple(() =>
        {
            Assert.That(intSize, Is.GreaterThan(0));
            Assert.That(stringSize, Is.GreaterThan(0));
            Assert.That(floatSize, Is.GreaterThan(0));
        });
    }
    
    #endregion
}

#region Test Classes

[ProtoPackable]
public partial class TestNodeClass
{
    [ProtoMember(1)] public int Test1 { get; set; }
}

[ProtoPackable]
public partial class TestNodeHybridClass
{
    [ProtoMember(1)] public int Test1 { get; set; }
    
    [ProtoMember(2, NodesWireType = WireType.VarInt)] public ProtoArray? Test2 { get; set; }
    
    [ProtoMember(3)] public ProtoObject? Test3 { get; set; }
}

#endregion
