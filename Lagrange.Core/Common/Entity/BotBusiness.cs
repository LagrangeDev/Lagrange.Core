namespace Lagrange.Core.Common.Entity
{
    public class BusinessCustom
    {
        public uint Type;

        public uint Level;

        public string? Icon;

        public uint IsYear;

        public uint IsPro;
    }

    public class BusinessCustomList
    {
        public List<BusinessCustom> BusinessLists { get; set; }

        public BusinessCustomList(List<BusinessCustom> businessLists)
        {
            BusinessLists = businessLists;
        }
    }
}