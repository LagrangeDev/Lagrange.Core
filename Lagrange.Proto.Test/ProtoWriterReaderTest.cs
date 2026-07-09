using System.Buffers;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Lagrange.Proto.Primitives;
using Lagrange.Proto.Serialization;
using Lagrange.Proto.Utility;

namespace Lagrange.Proto.Test;

[TestFixture]
public class ProtoWriterReaderTest
{
    private static readonly ulong[] VarInt64Values =
    [
        0,
        1,
        127,
        128,
        1UL << 28,
        1UL << 32,
        1UL << 56,
        1UL << 63,
        0x0102030405060708,
        0xFEDCBA9876543210,
        ulong.MaxValue
    ];

    private static readonly uint[] VarInt32Values =
    [
        0,
        1,
        127,
        128,
        16_383,
        16_384,
        2_097_151,
        2_097_152,
        268_435_455,
        268_435_456,
        1_742_816_827,
        uint.MaxValue
    ];

    private static readonly byte[] VarInt8Values = [0, 1, 127, 128, byte.MaxValue];

    private static readonly ushort[] VarInt16Values = [0, 1, 127, 128, 16_383, 16_384, ushort.MaxValue];

    #region VarInt Tests
    
    [Test]
    public void TestVarInt_SingleByte()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeVarInt(0);
        writer.EncodeVarInt(127);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int value1 = reader.DecodeVarInt<int>();
        int value2 = reader.DecodeVarInt<int>();
        
        bool isCompleted = reader.IsCompleted;
        
        Assert.Multiple(() =>
        {
            Assert.That(value1, Is.EqualTo(0));
            Assert.That(value2, Is.EqualTo(127));
            Assert.That(isCompleted, Is.True);
        });
    }
    
    [Test]
    public void TestVarInt_MultiByte()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeVarInt(128);
        writer.EncodeVarInt(16383);
        writer.EncodeVarInt(16384);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int value1 = reader.DecodeVarInt<int>();
        int value2 = reader.DecodeVarInt<int>();
        int value3 = reader.DecodeVarInt<int>();
        
        Assert.Multiple(() =>
        {
            Assert.That(value1, Is.EqualTo(128));
            Assert.That(value2, Is.EqualTo(16383));
            Assert.That(value3, Is.EqualTo(16384));
        });
    }
    
    [Test]
    public void TestVarInt_MaxValues()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeVarInt(byte.MaxValue);
        writer.EncodeVarInt(ushort.MaxValue);
        writer.EncodeVarInt(uint.MaxValue);
        writer.EncodeVarInt(ulong.MaxValue);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        byte byteVal = reader.DecodeVarInt<byte>();
        ushort ushortVal = reader.DecodeVarInt<ushort>();
        uint uintVal = reader.DecodeVarInt<uint>();
        ulong ulongVal = reader.DecodeVarInt<ulong>();
        
        Assert.Multiple(() =>
        {
            Assert.That(byteVal, Is.EqualTo(byte.MaxValue));
            Assert.That(ushortVal, Is.EqualTo(ushort.MaxValue));
            Assert.That(uintVal, Is.EqualTo(uint.MaxValue));
            Assert.That(ulongVal, Is.EqualTo(ulong.MaxValue));
        });
    }

    [Test]
    public void TestEncodeVarInt_UInt8AndUInt16MatchReference()
    {
        foreach (byte value in VarInt8Values)
        {
            AssertEncodedVarIntMatchesReference(value, value);
        }

        foreach (ushort value in VarInt16Values)
        {
            AssertEncodedVarIntMatchesReference(value, value);
        }
    }

    [Test]
    public void TestDecodeVarIntUInt8AndUInt16_UnsafePathMatchesReference()
    {
        foreach (byte value in VarInt8Values)
        {
            var reader = new ProtoReader(PadForUnsafePath(EncodeVarIntReference(value)));
            Assert.That(reader.DecodeVarInt<byte>(), Is.EqualTo(value));
        }

        foreach (ushort value in VarInt16Values)
        {
            var reader = new ProtoReader(PadForUnsafePath(EncodeVarIntReference(value)));
            Assert.That(reader.DecodeVarInt<ushort>(), Is.EqualTo(value));
        }
    }

    [TestCaseSource(nameof(VarInt32Values))]
    public void TestEncodeVarInt_UInt32MatchesReference(uint value)
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        writer.WriteRawByte(0xAA);
        writer.EncodeVarInt(value);
        writer.Flush();

        byte[] expected = [0xAA, .. EncodeVarIntReference(value)];
        Assert.That(buffer.WrittenMemory.ToArray(), Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(VarInt32Values))]
    public void TestDecodeVarIntUInt32_UnsafePathMatchesReference(uint value)
    {
        byte[] encoded = EncodeVarIntReference(value);
        byte[] padded = PadForUnsafePath(encoded);
        var reader = new ProtoReader(padded);

        uint decoded = reader.DecodeVarInt<uint>();

        Assert.That(decoded, Is.EqualTo(value));
    }

    [TestCaseSource(nameof(VarInt32Values))]
    public void TestDecodeVarIntUnsafePair_UInt32MatchesReference(uint value)
    {
        if (!Ssse3.X64.IsSupported && !AdvSimd.Arm64.IsSupported)
        {
            Assert.Ignore("SSSE3/NEON is not supported on this platform.");
        }

        byte[] first = EncodeVarIntReference(value);
        byte[] second = EncodeVarIntReference(42);
        byte[] padded = new byte[first.Length + second.Length + 16];
        first.CopyTo(padded, 0);
        second.CopyTo(padded, first.Length);
        var reader = new ProtoReader(padded);

        var (decodedFirst, decodedSecond) = reader.DecodeVarIntUnsafe<uint, uint>(padded);

        Assert.Multiple(() =>
        {
            Assert.That(decodedFirst, Is.EqualTo(value));
            Assert.That(decodedSecond, Is.EqualTo(42u));
        });
    }

    [TestCaseSource(nameof(VarInt64Values))]
    public void TestEncodeVarInt_UInt64MatchesReference(ulong value)
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        writer.WriteRawByte(0xAA);
        writer.EncodeVarInt(value);
        writer.Flush();

        byte[] expected = [0xAA, .. EncodeVarIntReference(value)];
        Assert.That(buffer.WrittenMemory.ToArray(), Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(VarInt64Values))]
    public void TestDecodeVarIntUInt64_UnsafePathMatchesReference(ulong value)
    {
        byte[] encoded = EncodeVarIntReference(value);
        byte[] padded = PadForUnsafePath(encoded);
        var reader = new ProtoReader(padded);

        ulong decoded = reader.DecodeVarInt<ulong>();

        Assert.That(decoded, Is.EqualTo(value));
    }

    [TestCaseSource(nameof(VarInt64Values))]
    public void TestDecodeVarIntUnsafePair_UInt64MatchesReference(ulong value)
    {
        if (!Ssse3.X64.IsSupported && !AdvSimd.Arm64.IsSupported)
        {
            Assert.Ignore("SSSE3/NEON is not supported on this platform.");
        }

        byte[] first = EncodeVarIntReference(value);
        byte[] second = EncodeVarIntReference(42);
        byte[] padded = new byte[first.Length + second.Length + 16];
        first.CopyTo(padded, 0);
        second.CopyTo(padded, first.Length);
        var reader = new ProtoReader(padded);

        var (decodedFirst, decodedSecond) = reader.DecodeVarIntUnsafe<ulong, uint>(padded);

        Assert.Multiple(() =>
        {
            Assert.That(decodedFirst, Is.EqualTo(value));
            Assert.That(decodedSecond, Is.EqualTo(42u));
        });
    }
    
    [Test]
    public void TestVarInt_NegativeNumbers()
    {
        // Negative numbers are encoded as unsigned in protobuf
        // They use zigzag encoding when NumberHandling.Signed is specified
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        // Write as unsigned (two's complement)
        writer.EncodeVarInt(unchecked((uint)-1));
        writer.EncodeVarInt(unchecked((uint)-128));
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        uint negOne = reader.DecodeVarInt<uint>();
        uint neg128 = reader.DecodeVarInt<uint>();
        
        Assert.Multiple(() =>
        {
            Assert.That(negOne, Is.EqualTo(uint.MaxValue));
            Assert.That(neg128, Is.EqualTo(uint.MaxValue - 127));
        });
    }
    
    [Test]
    public void TestDecodeVarIntUnsafe_DualValues()
    {
        if (!Sse3.IsSupported && !AdvSimd.Arm64.IsSupported)
        {
            Assert.Ignore("SSSE3/NEON is not supported on this platform.");
            return;
        }

        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeVarInt(42);
        writer.EncodeVarInt(123456);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        var (val1, val2) = reader.DecodeVarIntUnsafe<int, int>(buffer.WrittenMemory.Span);
        
        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(42));
            Assert.That(val2, Is.EqualTo(123456));
        });
    }
    
    [Test]
    public void TestDecodeVarIntUnsafe_MixedTypes()
    {
        if (!Sse3.IsSupported && !AdvSimd.Arm64.IsSupported)
        {
            Assert.Ignore("SSSE3/NEON is not supported on this platform.");
            return;
        }

        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        writer.EncodeVarInt(1145141919810L);
        writer.EncodeVarInt(114514);
        writer.Flush();

        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        var (val1, val2) = reader.DecodeVarIntUnsafe<long, int>(buffer.WrittenMemory.Span);

        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(1145141919810L));
            Assert.That(val2, Is.EqualTo(114514));
        });
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_BasicValues()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        // Test encoding two 32-bit values
        writer.EncodeTwo32VarIntUnsafe(42, 123456);
        writer.Flush();

        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int val1 = reader.DecodeVarInt<int>();
        int val2 = reader.DecodeVarInt<int>();
        bool isCompleted = reader.IsCompleted;

        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(42));
            Assert.That(val2, Is.EqualTo(123456));
            Assert.That(isCompleted, Is.True);
        });
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_ByteValues()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        writer.EncodeTwo32VarIntUnsafe((byte)255, (byte)128);
        writer.Flush();

        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        byte val1 = reader.DecodeVarInt<byte>();
        byte val2 = reader.DecodeVarInt<byte>();
        bool isCompleted = reader.IsCompleted;

        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(255));
            Assert.That(val2, Is.EqualTo(128));
            Assert.That(isCompleted, Is.True);
        });
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_ShortValues()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        writer.EncodeTwo32VarIntUnsafe((ushort)32767, (ushort)65535);
        writer.Flush();

        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        ushort val1 = reader.DecodeVarInt<ushort>();
        ushort val2 = reader.DecodeVarInt<ushort>();
        bool isCompleted = reader.IsCompleted;

        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(32767));
            Assert.That(val2, Is.EqualTo(65535));
            Assert.That(isCompleted, Is.True);
        });
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_MaxValues()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        writer.EncodeTwo32VarIntUnsafe(uint.MaxValue, uint.MaxValue);
        writer.Flush();

        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        uint val1 = reader.DecodeVarInt<uint>();
        uint val2 = reader.DecodeVarInt<uint>();
        bool isCompleted = reader.IsCompleted;

        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(uint.MaxValue));
            Assert.That(val2, Is.EqualTo(uint.MaxValue));
            Assert.That(isCompleted, Is.True);
        });
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_MixedTypes()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        // Test with different sized types
        writer.EncodeTwo32VarIntUnsafe((byte)200, (uint)1000000);
        writer.Flush();

        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        byte val1 = reader.DecodeVarInt<byte>();
        uint val2 = reader.DecodeVarInt<uint>();
        bool isCompleted = reader.IsCompleted;

        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(200));
            Assert.That(val2, Is.EqualTo(1000000));
            Assert.That(isCompleted, Is.True);
        });
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_ZeroValues()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        writer.EncodeTwo32VarIntUnsafe(0, 0);
        writer.Flush();

        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int val1 = reader.DecodeVarInt<int>();
        int val2 = reader.DecodeVarInt<int>();
        bool isCompleted = reader.IsCompleted;

        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(0));
            Assert.That(val2, Is.EqualTo(0));
            Assert.That(isCompleted, Is.True);
        });
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_SingleByteValues()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        // Values that fit in single byte (< 128)
        writer.EncodeTwo32VarIntUnsafe(127, 64);
        writer.Flush();

        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int val1 = reader.DecodeVarInt<int>();
        int val2 = reader.DecodeVarInt<int>();
        bool isCompleted = reader.IsCompleted;

        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(127));
            Assert.That(val2, Is.EqualTo(64));
            Assert.That(buffer.WrittenMemory.Length, Is.EqualTo(2)); // Should only use 2 bytes total
            Assert.That(isCompleted, Is.True);
        });
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_CompareWithRegularEncode()
    {
        var buffer1 = new ArrayBufferWriter<byte>();
        var writer1 = new ProtoWriter(buffer1);

        var buffer2 = new ArrayBufferWriter<byte>();
        var writer2 = new ProtoWriter(buffer2);

        uint value1 = 42;
        uint value2 = 123456;

        // Encode using regular method
        writer1.EncodeVarInt(value1);
        writer1.EncodeVarInt(value2);
        writer1.Flush();

        // Encode using Two32VarIntUnsafe
        writer2.EncodeTwo32VarIntUnsafe(value1, value2);
        writer2.Flush();

        // Both should produce identical output
        Assert.That(buffer2.WrittenMemory.ToArray(), Is.EqualTo(buffer1.WrittenMemory.ToArray()));
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_WithSsse3Fallback()
    {
        // This test verifies the non-SSSE3 fallback path works correctly
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        writer.EncodeTwo32VarIntUnsafe(12345, 67890);
        writer.Flush();

        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int val1 = reader.DecodeVarInt<int>();
        int val2 = reader.DecodeVarInt<int>();
        bool isCompleted = reader.IsCompleted;

        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(12345));
            Assert.That(val2, Is.EqualTo(67890));
            Assert.That(isCompleted, Is.True);
        });
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_RoundTripWithDecodeUnsafe()
    {
        if (!Ssse3.IsSupported && !AdvSimd.Arm64.IsSupported)
        {
            Assert.Ignore("SSSE3/NEON is not supported on this platform.");
            return;
        }

        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        uint value1 = 999999;
        uint value2 = 777777;

        writer.EncodeTwo32VarIntUnsafe(value1, value2);
        writer.Flush();

        // Decode using the dual decode method
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        var (decoded1, decoded2) = reader.DecodeVarIntUnsafe<uint, uint>(buffer.WrittenMemory.Span);

        Assert.Multiple(() =>
        {
            Assert.That(decoded1, Is.EqualTo(value1));
            Assert.That(decoded2, Is.EqualTo(value2));
        });
    }

    [Test]
    public void TestEncodeTwo32VarIntUnsafe_PerformanceComparison()
    {
        // Test to verify SIMD optimization provides correct results
        // Real performance testing should be done with BenchmarkDotNet
        const int iterations = 1000;

        var buffer1 = new ArrayBufferWriter<byte>(iterations * 10);
        var buffer2 = new ArrayBufferWriter<byte>(iterations * 10);

        var writer1 = new ProtoWriter(buffer1);
        var writer2 = new ProtoWriter(buffer2);

        // Generate test data
        var values = new (uint, uint)[iterations];
        for (int i = 0; i < iterations; i++)
        {
            values[i] = ((uint)(i * 7 + 13), (uint)(i * 11 + 17));
        }

        // Encode using regular method
        foreach (var (v1, v2) in values)
        {
            writer1.EncodeVarInt(v1);
            writer1.EncodeVarInt(v2);
        }
        writer1.Flush();

        // Encode using SIMD-optimized method
        foreach (var (v1, v2) in values)
        {
            writer2.EncodeTwo32VarIntUnsafe(v1, v2);
        }
        writer2.Flush();

        // Verify both produce identical output
        Assert.That(buffer2.WrittenMemory.ToArray(), Is.EqualTo(buffer1.WrittenMemory.ToArray()));

        // Verify we can decode all values correctly
        var reader = new ProtoReader(buffer2.WrittenMemory.Span);
        for (int i = 0; i < iterations; i++)
        {
            uint val1 = reader.DecodeVarInt<uint>();
            uint val2 = reader.DecodeVarInt<uint>();
            Assert.That(val1, Is.EqualTo(values[i].Item1));
            Assert.That(val2, Is.EqualTo(values[i].Item2));
        }

        Assert.That(reader.IsCompleted, Is.True);
    }

    #endregion
    
    #region Fixed32/Fixed64 Tests
    
    [Test]
    public void TestFixed32_AllTypes()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeFixed32(42);
        writer.EncodeFixed32(3.14f);
        writer.EncodeFixed32(uint.MaxValue);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int intVal = reader.DecodeFixed32<int>();
        float floatVal = reader.DecodeFixed32<float>();
        uint uintVal = reader.DecodeFixed32<uint>();
        
        Assert.Multiple(() =>
        {
            Assert.That(intVal, Is.EqualTo(42));
            Assert.That(floatVal, Is.EqualTo(3.14f).Within(0.001f));
            Assert.That(uintVal, Is.EqualTo(uint.MaxValue));
        });
    }
    
    [Test]
    public void TestFixed64_AllTypes()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeFixed64(42L);
        writer.EncodeFixed64(3.14159265359);
        writer.EncodeFixed64(ulong.MaxValue);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        long longVal = reader.DecodeFixed64<long>();
        double doubleVal = reader.DecodeFixed64<double>();
        ulong ulongVal = reader.DecodeFixed64<ulong>();
        
        Assert.Multiple(() =>
        {
            Assert.That(longVal, Is.EqualTo(42L));
            Assert.That(doubleVal, Is.EqualTo(3.14159265359).Within(0.000001));
            Assert.That(ulongVal, Is.EqualTo(ulong.MaxValue));
        });
    }
    
    #endregion
    
    #region String Tests
    
    [Test]
    public void TestString_Empty()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeString("");
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int length = reader.DecodeVarInt<int>();
        
        Assert.That(length, Is.EqualTo(0));
    }
    
    [Test]
    public void TestString_Ascii()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeString("Hello, World!");
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int length = reader.DecodeVarInt<int>();
        var span = reader.CreateSpan(length);
        string result = Encoding.UTF8.GetString(span);
        
        bool isCompleted = reader.IsCompleted;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo("Hello, World!"));
            Assert.That(isCompleted, Is.True);
        });
    }
    
    [Test]
    public void TestString_Unicode()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        string unicodeStr = "你好世界 🌍 Émoji 测试";
        writer.EncodeString(unicodeStr);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int length = reader.DecodeVarInt<int>();
        var span = reader.CreateSpan(length);
        string result = Encoding.UTF8.GetString(span);
        
        Assert.That(result, Is.EqualTo(unicodeStr));
    }
    
    [Test]
    public void TestString_LongString()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        string longStr = new string('A', 10000);
        writer.EncodeString(longStr);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int length = reader.DecodeVarInt<int>();
        var span = reader.CreateSpan(length);
        string result = Encoding.UTF8.GetString(span);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Length, Is.EqualTo(10000));
            Assert.That(result, Is.EqualTo(longStr));
        });
    }
    
    #endregion
    
    #region Bytes Tests
    
    [Test]
    public void TestBytes_Empty()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeBytes(Array.Empty<byte>());
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int length = reader.DecodeVarInt<int>();
        
        Assert.That(length, Is.EqualTo(0));
    }
    
    [Test]
    public void TestBytes_Small()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        byte[] data = { 1, 2, 3, 4, 5 };
        writer.EncodeBytes(data);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int length = reader.DecodeVarInt<int>();
        var span = reader.CreateSpan(length);
        
        byte[] spanArray = span.ToArray();
        
        Assert.Multiple(() =>
        {
            Assert.That(length, Is.EqualTo(5));
            Assert.That(spanArray, Is.EqualTo(data));
        });
    }
    
    [Test]
    public void TestBytes_Large()
    {
        var buffer = new SegmentBufferWriter();
        var writer = new ProtoWriter(buffer);
        
        byte[] largeData = new byte[1024 * 1024]; // 1MB
        Random.Shared.NextBytes(largeData);
        
        writer.EncodeBytes(largeData);
        writer.Flush();
        
        var memory = buffer.CreateReadOnlyMemory();
        var reader = new ProtoReader(memory.Span);
        int length = reader.DecodeVarInt<int>();
        var span = reader.CreateSpan(length);
        
        byte[] spanArray = span.ToArray();
        
        Assert.Multiple(() =>
        {
            Assert.That(length, Is.EqualTo(largeData.Length));
            Assert.That(spanArray, Is.EqualTo(largeData));
        });
        
        buffer.Dispose();
    }
    
    #endregion
    
    #region Raw Byte Operations
    
    [Test]
    public void TestWriteRawByte()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.WriteRawByte(0x00);
        writer.WriteRawByte(0xFF);
        writer.WriteRawByte(0x42);
        writer.Flush();
        
        Assert.That(buffer.WrittenMemory.ToArray(), Is.EqualTo(new byte[] { 0x00, 0xFF, 0x42 }));
    }
    
    [Test]
    public void TestWriteRawBytes()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        byte[] raw = { 0x01, 0x02, 0x03, 0x04 };
        writer.WriteRawBytes(raw);
        writer.Flush();
        
        Assert.That(buffer.WrittenMemory.ToArray(), Is.EqualTo(raw));
    }
    
    #endregion
    
    #region Skip Field Tests
    
    [Test]
    public void TestSkipField_VarInt()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeVarInt(12345);
        writer.EncodeVarInt(67890);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        reader.SkipField(WireType.VarInt);
        int value = reader.DecodeVarInt<int>();
        
        Assert.That(value, Is.EqualTo(67890));
    }
    
    [Test]
    public void TestSkipField_Fixed32()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeFixed32(3.14f);
        writer.EncodeFixed32(2.71f);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        reader.SkipField(WireType.Fixed32);
        float value = reader.DecodeFixed32<float>();
        
        Assert.That(value, Is.EqualTo(2.71f).Within(0.001f));
    }
    
    [Test]
    public void TestSkipField_Fixed64()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeFixed64(3.14159);
        writer.EncodeFixed64(2.71828);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        reader.SkipField(WireType.Fixed64);
        double value = reader.DecodeFixed64<double>();
        
        Assert.That(value, Is.EqualTo(2.71828).Within(0.00001));
    }
    
    [Test]
    public void TestSkipField_LengthDelimited()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeString("Skip this");
        writer.EncodeString("Read this");
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        reader.SkipField(WireType.LengthDelimited);
        int length = reader.DecodeVarInt<int>();
        var span = reader.CreateSpan(length);
        string value = Encoding.UTF8.GetString(span);
        
        Assert.That(value, Is.EqualTo("Read this"));
    }
    
    #endregion
    
    #region Writer State Tests
    
    [Test]
    public void TestWriter_BytesPending()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        Assert.That(writer.BytesPending, Is.EqualTo(0));
        
        writer.EncodeVarInt(42);
        Assert.That(writer.BytesPending, Is.EqualTo(1));
        
        writer.EncodeVarInt(300);
        Assert.That(writer.BytesPending, Is.EqualTo(3));
        
        writer.Flush();
        Assert.That(writer.BytesPending, Is.EqualTo(0));
        Assert.That(writer.BytesCommitted, Is.EqualTo(3));
    }
    
    [Test]
    public void TestWriter_Reset()
    {
        var buffer1 = new ArrayBufferWriter<byte>();
        var buffer2 = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer1);
        
        writer.EncodeVarInt(42);
        writer.Flush();
        
        writer.Reset(buffer2);
        writer.EncodeVarInt(100);
        writer.Flush();
        
        Assert.Multiple(() =>
        {
            Assert.That(buffer1.WrittenMemory.ToArray(), Is.EqualTo(new byte[] { 42 }));
            Assert.That(buffer2.WrittenMemory.ToArray(), Is.EqualTo(new byte[] { 100 }));
        });
    }
    
    #endregion
    
    #region Reader State Tests
    
    [Test]
    public void TestReader_IsCompleted()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeVarInt(42);
        writer.EncodeVarInt(100);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        
        bool isCompleted1 = reader.IsCompleted;
        Assert.That(isCompleted1, Is.False);
        
        reader.DecodeVarInt<int>();
        bool isCompleted2 = reader.IsCompleted;
        Assert.That(isCompleted2, Is.False);
        
        reader.DecodeVarInt<int>();
        bool isCompleted3 = reader.IsCompleted;
        Assert.That(isCompleted3, Is.True);
    }
    
    [Test]
    public void TestReader_Rewind()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        writer.EncodeVarInt(42);
        writer.EncodeVarInt(100);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        
        int first = reader.DecodeVarInt<int>();
        reader.Rewind(-1); // Go back one byte
        int second = reader.DecodeVarInt<int>();
        
        Assert.Multiple(() =>
        {
            Assert.That(first, Is.EqualTo(42));
            Assert.That(second, Is.EqualTo(42));
        });
    }
    
    #endregion
    
    #region Edge Cases and Error Handling
    
    [Test]
    public void TestVarInt_MaxBytesPerType()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        // Test maximum bytes needed for each type
        writer.EncodeVarInt((byte)0xFF);           // 2 bytes max for u8
        writer.EncodeVarInt((ushort)0xFFFF);       // 3 bytes max for u16
        writer.EncodeVarInt((uint)0xFFFFFFFF);     // 5 bytes max for u32
        writer.EncodeVarInt((ulong)0xFFFFFFFFFFFFFFFF); // 10 bytes max for u64
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        byte b = reader.DecodeVarInt<byte>();
        ushort us = reader.DecodeVarInt<ushort>();
        uint ui = reader.DecodeVarInt<uint>();
        ulong ul = reader.DecodeVarInt<ulong>();
        
        Assert.Multiple(() =>
        {
            Assert.That(b, Is.EqualTo(0xFF));
            Assert.That(us, Is.EqualTo(0xFFFF));
            Assert.That(ui, Is.EqualTo(0xFFFFFFFF));
            Assert.That(ul, Is.EqualTo(0xFFFFFFFFFFFFFFFF));
        });
    }
    
    [Test]
    public void TestWriter_GrowthBehavior()
    {
        var buffer = new ArrayBufferWriter<byte>(1); // Start with minimal size
        var writer = new ProtoWriter(buffer);
        
        // Force multiple growth operations
        byte[] largeData = new byte[2048];
        Random.Shared.NextBytes(largeData);
        
        writer.EncodeBytes(largeData);
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        int length = reader.DecodeVarInt<int>();
        var span = reader.CreateSpan(length);
        
        Assert.That(span.ToArray(), Is.EqualTo(largeData));
    }
    
    [Test]
    public void TestReader_InsufficientData()
    {
        byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }; // Malformed varint
        
        Assert.Throws<InvalidDataException>(() => 
        {
            var reader = new ProtoReader(data);
            reader.DecodeVarInt<int>();
        });
    }
    
    [Test]
    public void TestReader_InsufficientDataForFixed32()
    {
        byte[] data = { 0x01, 0x02, 0x03 }; // Only 3 bytes, need 4
        
        Assert.Throws<ArgumentOutOfRangeException>(() => 
        {
            var reader = new ProtoReader(data);
            reader.DecodeFixed32<int>();
        });
    }
    
    [Test]
    public void TestReader_InsufficientDataForFixed64()
    {
        byte[] data = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 }; // Only 7 bytes, need 8
        
        Assert.Throws<ArgumentOutOfRangeException>(() => 
        {
            var reader = new ProtoReader(data);
            reader.DecodeFixed64<long>();
        });
    }
    
    #endregion
    
    #region Performance Tests
    
    [Test]
    public void TestPerformance_LargeNumberOfVarInts()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        const int count = 10000;
        for (int i = 0; i < count; i++)
        {
            writer.EncodeVarInt(i);
        }
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        for (int i = 0; i < count; i++)
        {
            int value = reader.DecodeVarInt<int>();
            Assert.That(value, Is.EqualTo(i));
        }
        
        bool isCompleted = reader.IsCompleted;
        Assert.That(isCompleted, Is.True);
    }
    
    [Test]
    public void TestPerformance_MixedOperations()
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);
        
        for (int i = 0; i < 10; i++)
        {
            writer.EncodeVarInt(i);
            writer.EncodeFixed32((float)i * 1.5f);
            writer.EncodeBytes(new byte[] { (byte)i, (byte)(i + 1) });
        }
        writer.Flush();
        
        var reader = new ProtoReader(buffer.WrittenMemory.Span);
        for (int i = 0; i < 10; i++)
        {
            int intVal = reader.DecodeVarInt<int>();
            float floatVal = reader.DecodeFixed32<float>();
            int bytesLen = reader.DecodeVarInt<int>();
            byte[] bytesVal = reader.CreateSpan(bytesLen).ToArray();
            
            Assert.Multiple(() =>
            {
                Assert.That(intVal, Is.EqualTo(i));
                Assert.That(floatVal, Is.EqualTo((float)i * 1.5f).Within(0.001f));
                Assert.That(bytesVal, Is.EqualTo(new byte[] { (byte)i, (byte)(i + 1) }));
            });
        }
    }
    
    #endregion

    private static byte[] EncodeVarIntReference(ulong value)
    {
        var bytes = new List<byte>();

        do
        {
            byte next = (byte)(value & 0x7F);
            value >>= 7;
            if (value != 0) next |= 0x80;
            bytes.Add(next);
        }
        while (value != 0);

        return bytes.ToArray();
    }

    private static byte[] PadForUnsafePath(byte[] encoded)
    {
        byte[] padded = new byte[encoded.Length + 16];
        encoded.CopyTo(padded, 0);
        return padded;
    }

    private static void AssertEncodedVarIntMatchesReference<T>(T value, ulong expectedValue) where T : unmanaged, System.Numerics.INumber<T>
    {
        var buffer = new ArrayBufferWriter<byte>();
        var writer = new ProtoWriter(buffer);

        writer.WriteRawByte(0xAA);
        writer.EncodeVarInt(value);
        writer.Flush();

        byte[] expected = [0xAA, .. EncodeVarIntReference(expectedValue)];
        Assert.That(buffer.WrittenMemory.ToArray(), Is.EqualTo(expected));
    }
}
