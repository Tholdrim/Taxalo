using System.Reflection;
using ClosedXML.Excel;

namespace Taxalo
{
    public abstract class BaseXlsxWriter<TModel, TRecord> : IXlsxWriter<TModel>
    {
        protected PropertyInfo[] RecordProperties { get; } = typeof(TRecord).GetProperties();

        public virtual bool IsSupported(object model) => model is TModel;

        public void AddWorksheet(XLWorkbook workbook, TModel model)
        {
            var worksheetName = GetWorksheetName(workbook, model);
            var worksheet = workbook.AddWorksheet(worksheetName);
            var table = CreateTable(worksheet, model);

            ConfigureColumns(table);
            ConfigureHeaders(table);
        }

        protected abstract string GetWorksheetName(XLWorkbook workbook, TModel model);

        protected abstract IXLTable CreateTable(IXLWorksheet worksheet, TModel model);

        protected virtual void ConfigureHeaders(IXLTable table)
        {
            var headerRow = table.Range(1, 1, 1, RecordProperties.Length);

            headerRow.Style.SetHeaderStyle();
        }

        private void ConfigureColumns(IXLTable table)
        {
            foreach (var (property, index) in RecordProperties.WithIndex())
            {
                var attribute = property.GetCustomAttribute<XLCustomColumnAttribute>();

                if (attribute == null)
                {
                    continue;
                }

                var column = table.Column(index + 1).WorksheetColumn();

                column.Width = attribute.Width;
                column.Style.SetStyle(attribute.Format);
            }
        }
    }
}
