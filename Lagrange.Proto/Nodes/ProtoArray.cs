using Lagrange.Proto.Primitives;
using Lagrange.Proto.Serialization;
using Lagrange.Proto.Utility;

namespace Lagrange.Proto.Nodes;

public sealed partial class ProtoArray : ProtoNode
{
    private readonly List<ProtoNode> _list = [];

    public ProtoArray(WireType wireType, params ReadOnlySpan<ProtoNode> nodes) : base(wireType)
    {
        _list.AddRange(nodes);
        foreach (var node in nodes) node.AssignParent(this);
    }
    
    public ProtoArray(WireType wireType, params ProtoNode[] nodes) : base(wireType)
    {
        _list.AddRange(nodes);
        foreach (var node in nodes) node.AssignParent(this);
    }
    
    public ProtoArray(WireType wireType) : base(wireType) { }

    public IEnumerable<T> GetValues<T>()
    {
        foreach (var item in _list)
        {
            yield return item.GetValue<T>();
        }
    }
    
    public override void WriteTo(int field, ProtoWriter writer)
    {
        bool first = true;
        
        foreach (var node in _list)
        {
            if (first) first = false;
            else writer.EncodeVarInt(ProtoHelper.GetTag(field, node.WireType));

            node.WriteTo(field, writer);
        }
    }
    
    public override int Measure(int field)
    {
        if (_list.Count == 0) return 0;
        
        int size = ProtoHelper.GetVarIntLength(ProtoHelper.GetTag(field, WireType)) * (_list.Count - 1);
        foreach (var node in _list)
        {
            size += node.Measure(field);
        }
        return size;
    }
    
    private protected override ProtoNode GetItem(int index)
    {
        return _list[index];
    }

    private protected override void SetItem(int index, ProtoNode value)
    {
        value.AssignParent(this);
        DetachParent(_list[index]);
        _list[index] = value;
    }
}
