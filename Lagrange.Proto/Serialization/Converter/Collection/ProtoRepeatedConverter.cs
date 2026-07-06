using Lagrange.Proto.Primitives;
using Lagrange.Proto.Serialization.Metadata;
using Lagrange.Proto.Utility;

namespace Lagrange.Proto.Serialization.Converter;

public abstract class ProtoRepeatedConverter<TCollection, TElement> : ProtoConverter<TCollection> where TCollection : ICollection<TElement>
{
    private readonly ProtoConverter<TElement> _converter = ProtoTypeResolver.GetConverter<TElement>();

    public override bool ShouldSerialize(TCollection value, bool ignoreDefaultValue) => value is { Count: > 0 };

    public override void Write(int field, WireType wireType, ProtoWriter writer, TCollection value)
    {
        uint tag = ProtoHelper.GetTag(field, wireType);
        bool first = true;
        
        foreach (var item in value)
        {
            if (first) first = false;
            else writer.EncodeVarInt(tag);
            _converter.Write(field, wireType, writer, item);
        }
    }

    public override void WriteWithNumberHandling(int field, WireType wireType, ProtoWriter writer, TCollection value, ProtoNumberHandling numberHandling)
    {
        uint tag = ProtoHelper.GetTag(field, wireType);
        bool first = true;
        
        foreach (var item in value)
        {
            if (first) first = false;
            else writer.EncodeVarInt(tag);
            _converter.WriteWithNumberHandling(field, wireType, writer, item, numberHandling);
        }
    }

    public override int Measure(int field, WireType wireType, TCollection value)
    {
        uint tag = ProtoHelper.GetTag(field, wireType);
        int size = ProtoHelper.GetVarIntLength(tag) * (value.Count - 1); // the length of the first item is not counted as it would be added by the caller
        
        foreach (var item in value)
        {
            size += _converter.Measure(field, wireType, item);
        }

        return size;
    }

    public override TCollection Read(int field, WireType wireType, ref ProtoReader reader)
    {
        var collection = Create();
        object? state = CreateState();

        int tag;
        while (true)
        {
            var item = _converter.Read(field, wireType, ref reader);
            Add(item, collection, state);
            if (reader.IsCompleted) return Finalize(collection, state);
            if ((tag = reader.DecodeVarInt<int>() >> 3) != field) break;
        }

        reader.Rewind(-ProtoHelper.GetVarIntLength(ProtoHelper.GetTag(tag, WireType.VarInt)));

        return Finalize(collection, state);
    }
    
    public override TCollection ReadWithNumberHandling(int field, WireType wireType, ref ProtoReader reader, ProtoNumberHandling numberHandling)
    {
        var collection = Create();
        object? state = CreateState();

        int tag;
        while (true)
        {
            var item = _converter.ReadWithNumberHandling(field, wireType, ref reader, numberHandling);
            Add(item, collection, state);
            if ((tag = reader.DecodeVarInt<int>() >> 3) != field) break;
        }

        reader.Rewind(-ProtoHelper.GetVarIntLength(ProtoHelper.GetTag(tag, WireType.VarInt)));

        return Finalize(collection, state);
    }

    private protected abstract TCollection Create();
    
    private protected virtual object? CreateState() => null;
    
    private protected abstract void Add(TElement item, TCollection collection, object? state);
    
    private protected virtual TCollection Finalize(TCollection collection, object? state) => collection;
}
