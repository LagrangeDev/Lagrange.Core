namespace Lagrange.Core.Core.Context.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal class BusinessLogicAttribute : Attribute
{
    public string Name { get; }
    
    public string Description { get; }
    
    public BusinessLogicAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }   
}