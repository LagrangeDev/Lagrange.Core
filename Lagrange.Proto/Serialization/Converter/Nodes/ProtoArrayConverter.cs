using Lagrange.Proto.Nodes;
using Lagrange.Proto.Primitives;
using Lagrange.Proto.Serialization.Metadata;
using Lagrange.Proto.Utility;

namespace Lagrange.Proto.Serialization.Converter;

public class ProtoArrayConverter : ProtoConverter<ProtoArray>
{
    public override void Write(int field, WireType wireType, ProtoWriter writer, ProtoArray value)
    {
        value.WriteTo(field, writer);
    }

    public override int Measure(int field, WireType wireType, ProtoArray value)
    {
        return value.Measure(field);
    }

    public override ProtoArray Read(int field, WireType wireType, ref ProtoReader reader)
    {
        var array = new ProtoArray(wireType);
        var converter = ProtoTypeResolver.GetConverter<ProtoRawValue>();

        int tag;
        while (true)
        {
            var value = converter.Read(field, wireType, ref reader);
            array.Add(new ProtoValue<ProtoRawValue>(value, wireType));
            
            if ((tag = reader.DecodeVarInt<int>() >> 3) != field) break;
        }

        reader.Rewind(-ProtoHelper.GetVarIntLength(ProtoHelper.GetTag(tag, WireType.VarInt)));
        return array;
    }
}
