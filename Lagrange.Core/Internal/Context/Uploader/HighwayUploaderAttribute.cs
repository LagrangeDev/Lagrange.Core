namespace Lagrange.Core.Internal.Context.Uploader;

[AttributeUsage(AttributeTargets.Class)]
internal class HighwayUploaderAttribute : Attribute
{
    public Type Entity { get; }
    
    public HighwayUploaderAttribute(Type entity) => Entity = entity;
}