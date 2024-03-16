using ClosedXML.Excel;

namespace Taxalo.Integrations.Nbp
{
    internal class NbpWriter : BaseXlsxWriter<CurrencyExchangeRates, NbpXlsxRecord>
    {
        protected override IXLTable CreateTable(IXLWorksheet worksheet, CurrencyExchangeRates model)
        {
            var cell = worksheet.Cell(1, 1);

            return cell.InsertTable(model.ExchangeRates.Select(r => r.ToXlsxRecord()));
        }

        protected override string GetWorksheetName(XLWorkbook workbook, CurrencyExchangeRates model)
        {
            return $"Kursy średnie {model.Currency}";
        }
    }
}
