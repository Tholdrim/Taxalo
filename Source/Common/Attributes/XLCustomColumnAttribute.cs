using ClosedXML.Attributes;

namespace Taxalo
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class XLCustomColumnAttribute : XLColumnAttribute
    {
        public XLCustomColumnAttribute(string header)
        {
            Header = header;
        }

        public required double Width { get; init; }

        public CellFormat Format { get; init; } = CellFormat.Text;
    }
}
