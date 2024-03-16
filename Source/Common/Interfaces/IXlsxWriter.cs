using ClosedXML.Excel;

namespace Taxalo
{
    public interface IXlsxWriter
    {
        bool IsSupported(object model);

        void AddWorksheet(XLWorkbook workbook, object model);
    }

    public interface IXlsxWriter<TModel> : IXlsxWriter
    {
        void AddWorksheet(XLWorkbook workbook, TModel model);

        void IXlsxWriter.AddWorksheet(XLWorkbook workbook, object model)
        {
            if (model is not TModel supportedModel)
            {
                throw new UnsupportedModelException();
            }

            AddWorksheet(workbook, supportedModel);
        }
    }
}
