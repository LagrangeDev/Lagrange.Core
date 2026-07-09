using System.Buffers;
using Lagrange.Proto.Nodes;
using Lagrange.Proto.Serialization;

namespace Lagrange.Proto.Test;

[TestFixture]
public class ProtoSerializationTest
{
    #region Basic Serialization Tests
    
    [Test]
    public void TestSerialize_SimpleClass()
    {
        var obj = new SimpleTestClass
        {
            IntValue = 42,
            StringValue = "Hello World",
            FloatValue = 3.14f
        };
        
        byte[] bytes = ProtoSerializer.Serialize(obj);
        SimpleTestClass deserialized = ProtoSerializer.Deserialize<SimpleTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.IntValue, Is.EqualTo(obj.IntValue));
            Assert.That(deserialized.StringValue, Is.EqualTo(obj.StringValue));
            Assert.That(deserialized.FloatValue, Is.EqualTo(obj.FloatValue).Within(0.001f));
        });
    }
    
    [Test]
    public void TestSerializeProtoPackable_SimpleClass()
    {
        var obj = new SimpleProtoPackableClass
        {
            IntValue = 42,
            StringValue = "Hello World",
            FloatValue = 3.14f
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        SimpleProtoPackableClass deserialized = ProtoSerializer.DeserializeProtoPackable<SimpleProtoPackableClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.IntValue, Is.EqualTo(obj.IntValue));
            Assert.That(deserialized.StringValue, Is.EqualTo(obj.StringValue));
            Assert.That(deserialized.FloatValue, Is.EqualTo(obj.FloatValue).Within(0.001f));
        });
    }
    
    [Test]
    public void TestSerialize_ToBuffer()
    {
        var obj = new SimpleProtoPackableClass
        {
            IntValue = 100,
            StringValue = "Buffer Test",
            FloatValue = 2.71f
        };
        
        var buffer = new ArrayBufferWriter<byte>();
        ProtoSerializer.SerializeProtoPackable(buffer, obj);
        
        SimpleProtoPackableClass deserialized = ProtoSerializer.DeserializeProtoPackable<SimpleProtoPackableClass>(buffer.WrittenMemory.ToArray());
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.IntValue, Is.EqualTo(obj.IntValue));
            Assert.That(deserialized.StringValue, Is.EqualTo(obj.StringValue));
            Assert.That(deserialized.FloatValue, Is.EqualTo(obj.FloatValue).Within(0.001f));
        });
    }
    
    #endregion
    
    #region Nested Object Tests
    
    [Test]
    public void TestSerialize_NestedObjects()
    {
        var obj = new NestedTestClass
        {
            Id = 1,
            Name = "Parent",
            Child = new NestedChildClass
            {
                ChildId = 2,
                ChildName = "Child",
                GrandChild = new GrandChildClass
                {
                    Value = "GrandChild"
                }
            }
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        NestedTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<NestedTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.Id, Is.EqualTo(obj.Id));
            Assert.That(deserialized.Name, Is.EqualTo(obj.Name));
            Assert.That(deserialized.Child.ChildId, Is.EqualTo(obj.Child.ChildId));
            Assert.That(deserialized.Child.ChildName, Is.EqualTo(obj.Child.ChildName));
            Assert.That(deserialized.Child.GrandChild.Value, Is.EqualTo(obj.Child.GrandChild.Value));
        });
    }
    
    #endregion
    
    #region Collection Tests
    
    [Test]
    public void TestSerialize_Arrays()
    {
        var obj = new ArrayTestClass
        {
            IntArray = new[] { 1, 2, 3, 4, 5 },
            StringArray = new[] { "One", "Two", "Three" },
            FloatArray = new[] { 1.1f, 2.2f, 3.3f },
            ByteArray = new byte[] { 0xFF, 0x00, 0xAB }
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        ArrayTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<ArrayTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.IntArray, Is.EqualTo(obj.IntArray));
            Assert.That(deserialized.StringArray, Is.EqualTo(obj.StringArray));
            Assert.That(deserialized.FloatArray, Is.EqualTo(obj.FloatArray));
            Assert.That(deserialized.ByteArray, Is.EqualTo(obj.ByteArray));
        });
    }
    
    [Test]
    public void TestSerialize_Lists()
    {
        var obj = new ListTestClass
        {
            IntList = new List<int> { 10, 20, 30 },
            StringList = new List<string> { "Alpha", "Beta", "Gamma" },
            DoubleList = new List<double> { 1.23, 4.56, 7.89 }
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        ListTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<ListTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.IntList, Is.EqualTo(obj.IntList));
            Assert.That(deserialized.StringList, Is.EqualTo(obj.StringList));
            Assert.That(deserialized.DoubleList, Is.EqualTo(obj.DoubleList));
        });
    }
    
    [Test]
    public void TestSerialize_Dictionary()
    {
        var obj = new DictionaryTestClass
        {
            StringIntDict = new Dictionary<string, int>
            {
                { "One", 1 },
                { "Two", 2 },
                { "Three", 3 }
            },
            IntStringDict = new Dictionary<int, string>
            {
                { 1, "First" },
                { 2, "Second" },
                { 3, "Third" }
            }
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        DictionaryTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<DictionaryTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.StringIntDict, Is.EqualTo(obj.StringIntDict));
            Assert.That(deserialized.IntStringDict, Is.EqualTo(obj.IntStringDict));
        });
    }
    
    #endregion
    
    #region Number Handling Tests
    
    [Test]
    public void TestSerialize_SignedNumbers()
    {
        var obj = new SignedNumberTestClass
        {
            SignedInt = -123456,
            SignedLong = -9876543210L,
            UnsignedInt = 1742816827,
            UnsignedLong = 9876543210L,
            Fixed32Value = -42,
            Fixed64Value = -999999999999L
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        SignedNumberTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<SignedNumberTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.SignedInt, Is.EqualTo(obj.SignedInt));
            Assert.That(deserialized.SignedLong, Is.EqualTo(obj.SignedLong));
            Assert.That(deserialized.UnsignedInt, Is.EqualTo(obj.UnsignedInt));
            Assert.That(deserialized.UnsignedLong, Is.EqualTo(obj.UnsignedLong));
            Assert.That(deserialized.Fixed32Value, Is.EqualTo(obj.Fixed32Value));
            Assert.That(deserialized.Fixed64Value, Is.EqualTo(obj.Fixed64Value));
        });
    }
    
    #endregion
    
    #region Memory Types Tests
    
    [Test]
    public void TestSerialize_MemoryTypes()
    {
        byte[] byteData = { 1, 2, 3, 4, 5 };
        char[] charData = { 'H', 'e', 'l', 'l', 'o' };
        
        var obj = new MemoryTestClass
        {
            ByteMemory = byteData.AsMemory(),
            CharMemory = charData.AsMemory(),
            ReadOnlyByteMemory = byteData.AsMemory(),
            ReadOnlyCharMemory = charData.AsMemory()
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        MemoryTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<MemoryTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.ByteMemory.ToArray(), Is.EqualTo(byteData));
            Assert.That(deserialized.CharMemory.ToArray(), Is.EqualTo(charData));
            Assert.That(deserialized.ReadOnlyByteMemory.ToArray(), Is.EqualTo(byteData));
            Assert.That(deserialized.ReadOnlyCharMemory.ToArray(), Is.EqualTo(charData));
        });
    }
    
    #endregion
    
    #region Nullable Types Tests
    
    [Test]
    public void TestSerialize_NullableTypes_WithValues()
    {
        var obj = new NullableTestClass
        {
            NullableInt = 42,
            NullableString = "Not Null",
            NullableBool = true,
            NullableDouble = 3.14159,
            NullableEnum = TestEnumType.Value2
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        NullableTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<NullableTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.NullableInt, Is.EqualTo(obj.NullableInt));
            Assert.That(deserialized.NullableString, Is.EqualTo(obj.NullableString));
            Assert.That(deserialized.NullableBool, Is.EqualTo(obj.NullableBool));
            Assert.That(deserialized.NullableDouble, Is.EqualTo(obj.NullableDouble));
            Assert.That(deserialized.NullableEnum, Is.EqualTo(obj.NullableEnum));
        });
    }
    
    [Test]
    public void TestSerialize_NullableTypes_WithNulls()
    {
        var obj = new NullableTestClass
        {
            NullableInt = null,
            NullableString = null,
            NullableBool = null,
            NullableDouble = null,
            NullableEnum = null
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        NullableTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<NullableTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.NullableInt, Is.Null);
            Assert.That(deserialized.NullableString, Is.Null);
            Assert.That(deserialized.NullableBool, Is.Null);
            Assert.That(deserialized.NullableDouble, Is.Null);
            Assert.That(deserialized.NullableEnum, Is.Null);
        });
    }
    
    #endregion
    
    #region Enum Tests
    
    [Test]
    public void TestSerialize_Enums()
    {
        var obj = new EnumTestClass
        {
            EnumValue = TestEnumType.Value3,
            EnumArray = new[] { TestEnumType.Value1, TestEnumType.Value2, TestEnumType.Value3 }
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        EnumTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<EnumTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.EnumValue, Is.EqualTo(obj.EnumValue));
            Assert.That(deserialized.EnumArray, Is.EqualTo(obj.EnumArray));
        });
    }
    
    #endregion
    
    #region IgnoreDefaultFields Tests
    
    [Test]
    public void TestSerialize_IgnoreDefaultFields()
    {
        var obj = new IgnoreDefaultTestClass
        {
            // Leave all fields at default values
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        
        // When ignoring defaults, the serialized size should be minimal
        Assert.That(bytes.Length, Is.LessThan(10));
        
        IgnoreDefaultTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<IgnoreDefaultTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.IntValue, Is.EqualTo(0));
            Assert.That(deserialized.StringValue, Is.EqualTo(string.Empty));
            Assert.That(deserialized.BoolValue, Is.False);
        });
    }
    
    [Test]
    public void TestSerialize_IgnoreDefaultFields_WithNonDefaults()
    {
        var obj = new IgnoreDefaultTestClass
        {
            IntValue = 42,
            StringValue = "Not Default",
            BoolValue = true
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        IgnoreDefaultTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<IgnoreDefaultTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.IntValue, Is.EqualTo(obj.IntValue));
            Assert.That(deserialized.StringValue, Is.EqualTo(obj.StringValue));
            Assert.That(deserialized.BoolValue, Is.EqualTo(obj.BoolValue));
        });
    }
    
    #endregion
    
    #region Hybrid Proto Node Tests
    
    [Test]
    public void TestSerialize_HybridProtoNodes()
    {
        var obj = new HybridTestClass
        {
            RegularField = 42,
            ProtoArrayField = new ProtoArray(WireType.VarInt, 1, 2, 3, 4, 5),
            ProtoObjectField = new ProtoObject
            {
                { 1, "Test" },
                { 2, 100 }
            }
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        HybridTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<HybridTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.RegularField, Is.EqualTo(obj.RegularField));
            Assert.That(deserialized.ProtoArrayField?.GetValues<int>().ToArray(), 
                Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));
            Assert.That(deserialized.ProtoObjectField?[1].GetValue<string>(), Is.EqualTo("Test"));
            Assert.That(deserialized.ProtoObjectField?[2].GetValue<int>(), Is.EqualTo(100));
        });
    }
    
    #endregion
    
    #region Edge Cases
    
    [Test]
    public void TestSerialize_EmptyClass()
    {
        var obj = new EmptyTestClass();
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        EmptyTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<EmptyTestClass>(bytes);
        
        Assert.That(deserialized, Is.Not.Null);
    }
    
    [Test]
    public void TestSerialize_EmptyCollections()
    {
        var obj = new ArrayTestClass
        {
            IntArray = Array.Empty<int>(),
            StringArray = Array.Empty<string>(),
            FloatArray = Array.Empty<float>(),
            ByteArray = Array.Empty<byte>()
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        ArrayTestClass deserialized = ProtoSerializer.DeserializeProtoPackable<ArrayTestClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.IntArray, Is.Empty);
            Assert.That(deserialized.StringArray, Is.Empty);
            Assert.That(deserialized.FloatArray, Is.Empty);
            Assert.That(deserialized.ByteArray, Is.Empty);
        });
    }
    
    [Test]
    public void TestSerialize_LargeFieldNumbers()
    {
        var obj = new LargeFieldNumberClass
        {
            Field1 = "First",
            Field1000 = "Middle",
            Field1000000 = "Large"
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        LargeFieldNumberClass deserialized = ProtoSerializer.DeserializeProtoPackable<LargeFieldNumberClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.Field1, Is.EqualTo(obj.Field1));
            Assert.That(deserialized.Field1000, Is.EqualTo(obj.Field1000));
            Assert.That(deserialized.Field1000000, Is.EqualTo(obj.Field1000000));
        });
    }
    
    [Test]
    public void TestSerialize_VeryLongStrings()
    {
        var obj = new SimpleProtoPackableClass
        {
            IntValue = 1,
            StringValue = new string('A', 100000), // 100K characters
            FloatValue = 1.0f
        };
        
        byte[] bytes = ProtoSerializer.SerializeProtoPackable(obj);
        SimpleProtoPackableClass deserialized = ProtoSerializer.DeserializeProtoPackable<SimpleProtoPackableClass>(bytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.StringValue.Length, Is.EqualTo(100000));
            Assert.That(deserialized.StringValue, Is.EqualTo(obj.StringValue));
        });
    }
    
    #endregion
    
    #region Reflection vs Source Generated Comparison
    
    [Test]
    public void TestSerialize_ReflectionVsSourceGen_Equality()
    {
        var obj = new ComparisionTestClass
        {
            Test1 = 114514,
            Test2 = "Test String",
            Test3 = 3.14f,
            Test4 = 2.71828,
            Test5 = 42,
            Test6 = "Another String",
            Test7 = new byte[] { 1, 2, 3, 4, 5 },
            Test8 = true,
            Test9 = 9876543210L
        };
        
        byte[] reflectionBytes = ProtoSerializer.Serialize(obj);
        byte[] srcGenBytes = ProtoSerializer.SerializeProtoPackable(obj);
        
        Assert.That(reflectionBytes, Is.EqualTo(srcGenBytes), 
            "Reflection and source-generated serialization should produce identical output");
        
        ComparisionTestClass reflectionDeserialized = ProtoSerializer.Deserialize<ComparisionTestClass>(reflectionBytes);
        ComparisionTestClass srcGenDeserialized = ProtoSerializer.DeserializeProtoPackable<ComparisionTestClass>(srcGenBytes);
        
        Assert.Multiple(() =>
        {
            Assert.That(reflectionDeserialized.Test1, Is.EqualTo(srcGenDeserialized.Test1));
            Assert.That(reflectionDeserialized.Test2, Is.EqualTo(srcGenDeserialized.Test2));
            Assert.That(reflectionDeserialized.Test3, Is.EqualTo(srcGenDeserialized.Test3));
            Assert.That(reflectionDeserialized.Test4, Is.EqualTo(srcGenDeserialized.Test4));
            Assert.That(reflectionDeserialized.Test5, Is.EqualTo(srcGenDeserialized.Test5));
            Assert.That(reflectionDeserialized.Test6, Is.EqualTo(srcGenDeserialized.Test6));
            Assert.That(reflectionDeserialized.Test7, Is.EqualTo(srcGenDeserialized.Test7));
            Assert.That(reflectionDeserialized.Test8, Is.EqualTo(srcGenDeserialized.Test8));
            Assert.That(reflectionDeserialized.Test9, Is.EqualTo(srcGenDeserialized.Test9));
        });
    }
    
    #endregion
}

#region Test Classes

public class SimpleTestClass
{
    [ProtoMember(1)] public int IntValue { get; set; }
    [ProtoMember(2)] public string StringValue { get; set; } = string.Empty;
    [ProtoMember(3)] public float FloatValue { get; set; }
}

[ProtoPackable]
public partial class SimpleProtoPackableClass
{
    [ProtoMember(1)] public int IntValue { get; set; }
    [ProtoMember(2)] public string StringValue { get; set; } = string.Empty;
    [ProtoMember(3)] public float FloatValue { get; set; }
}

[ProtoPackable]
public partial class NestedTestClass
{
    [ProtoMember(1)] public int Id { get; set; }
    [ProtoMember(2)] public string Name { get; set; } = string.Empty;
    [ProtoMember(3)] public NestedChildClass Child { get; set; } = new();
}

[ProtoPackable]
public partial class NestedChildClass
{
    [ProtoMember(1)] public int ChildId { get; set; }
    [ProtoMember(2)] public string ChildName { get; set; } = string.Empty;
    [ProtoMember(3)] public GrandChildClass GrandChild { get; set; } = new();
}

[ProtoPackable]
public partial class GrandChildClass
{
    [ProtoMember(1)] public string Value { get; set; } = string.Empty;
}

[ProtoPackable]
public partial class ArrayTestClass
{
    [ProtoMember(1)] public int[] IntArray { get; set; } = Array.Empty<int>();
    [ProtoMember(2)] public string[] StringArray { get; set; } = Array.Empty<string>();
    [ProtoMember(3)] public float[] FloatArray { get; set; } = Array.Empty<float>();
    [ProtoMember(4)] public byte[] ByteArray { get; set; } = Array.Empty<byte>();
}

[ProtoPackable]
public partial class ListTestClass
{
    [ProtoMember(1)] public List<int> IntList { get; set; } = new();
    [ProtoMember(2)] public List<string> StringList { get; set; } = new();
    [ProtoMember(3)] public List<double> DoubleList { get; set; } = new();
}

[ProtoPackable]
public partial class DictionaryTestClass
{
    [ProtoMember(1)] public Dictionary<string, int> StringIntDict { get; set; } = new();
    [ProtoMember(2)] public Dictionary<int, string> IntStringDict { get; set; } = new();
}

[ProtoPackable]
public partial class SignedNumberTestClass
{
    [ProtoMember(1, NumberHandling = ProtoNumberHandling.Signed)] 
    public int SignedInt { get; set; }
    
    [ProtoMember(2, NumberHandling = ProtoNumberHandling.Signed)] 
    public long SignedLong { get; set; }
    
    [ProtoMember(3)] 
    public uint UnsignedInt { get; set; }
    
    [ProtoMember(4)] 
    public ulong UnsignedLong { get; set; }
    
    [ProtoMember(5, NumberHandling = ProtoNumberHandling.Fixed32)] 
    public int Fixed32Value { get; set; }
    
    [ProtoMember(6, NumberHandling = ProtoNumberHandling.Fixed64)] 
    public long Fixed64Value { get; set; }
}

[ProtoPackable]
public partial class MemoryTestClass
{
    [ProtoMember(1)] public Memory<byte> ByteMemory { get; set; }
    [ProtoMember(2)] public Memory<char> CharMemory { get; set; }
    [ProtoMember(3)] public ReadOnlyMemory<byte> ReadOnlyByteMemory { get; set; }
    [ProtoMember(4)] public ReadOnlyMemory<char> ReadOnlyCharMemory { get; set; }
}

[ProtoPackable]
public partial class NullableTestClass
{
    [ProtoMember(1)] public int? NullableInt { get; set; }
    [ProtoMember(2)] public string? NullableString { get; set; }
    [ProtoMember(3)] public bool? NullableBool { get; set; }
    [ProtoMember(4)] public double? NullableDouble { get; set; }
    [ProtoMember(5)] public TestEnumType? NullableEnum { get; set; }
}

public enum TestEnumType
{
    Value1 = 1,
    Value2 = 2,
    Value3 = 3
}

[ProtoPackable]
public partial class EnumTestClass
{
    [ProtoMember(1)] public TestEnumType EnumValue { get; set; }
    [ProtoMember(2)] public TestEnumType[] EnumArray { get; set; } = Array.Empty<TestEnumType>();
}

[ProtoPackable(IgnoreDefaultFields = true)]
public partial class IgnoreDefaultTestClass
{
    [ProtoMember(1)] public int IntValue { get; set; }
    [ProtoMember(2)] public string StringValue { get; set; } = string.Empty;
    [ProtoMember(3)] public bool BoolValue { get; set; }
}

[ProtoPackable]
public partial class HybridTestClass
{
    [ProtoMember(1)] public int RegularField { get; set; }
    [ProtoMember(2, NodesWireType = WireType.VarInt)] public ProtoArray? ProtoArrayField { get; set; }
    [ProtoMember(3)] public ProtoObject? ProtoObjectField { get; set; }
}

[ProtoPackable]
public partial class EmptyTestClass
{
}

[ProtoPackable]
public partial class LargeFieldNumberClass
{
    [ProtoMember(1)] public string Field1 { get; set; } = string.Empty;
    [ProtoMember(1000)] public string Field1000 { get; set; } = string.Empty;
    [ProtoMember(1000000)] public string Field1000000 { get; set; } = string.Empty;
}

[ProtoPackable]
public partial class ComparisionTestClass
{
    [ProtoMember(1, NumberHandling = ProtoNumberHandling.Signed)] 
    public int Test1 { get; set; }
    
    [ProtoMember(2)] public string Test2 { get; set; } = string.Empty;
    
    [ProtoMember(3)] public float Test3 { get; set; }
    
    [ProtoMember(4)] public double Test4 { get; set; }
    
    [ProtoMember(5)] public int? Test5 { get; set; }
    
    [ProtoMember(6)] public string? Test6 { get; set; }
    
    [ProtoMember(7)] public byte[] Test7 { get; set; } = Array.Empty<byte>();
    
    [ProtoMember(8)] public bool Test8 { get; set; }
    
    [ProtoMember(9)] public long Test9 { get; set; }
}

#endregion
