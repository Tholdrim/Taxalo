using ClosedXML.Excel;

namespace Taxalo
{
    internal static class XLStyleExtensions
    {
        public static IXLStyle SetHeaderStyle(this IXLStyle style)
        {
            return style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Fill.SetBackgroundColor(XLColor.LightSteelBlue)
                .Font.SetFontSize(14.0);
        }

        public static IXLStyle SetStyle(this IXLStyle style, CellFormat format, string? isoCurrency = null, int precision = 8) => format switch
        {
            CellFormat.Date       => ApplyDateStyle(style),
            CellFormat.Decimal    => ApplyDecimalStyle(style, precision),
            CellFormat.IsoDecimal => ApplyDecimalStyle(style, precision, isoCurrency ?? ExchangeRate.DefaultCurrency),
            CellFormat.Text       => ApplyTextStyle(style),
            _                     => throw new ArgumentOutOfRangeException(nameof(format))
        };

        private static IXLStyle ApplyDateStyle(IXLStyle style)
        {
            return style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .DateFormat.SetFormat("YYYY-MM-DD");
        }

        private static IXLStyle ApplyDecimalStyle(IXLStyle style, int precision, string? currency = null)
        {
            var precisionPart = new string('0', precision);
            var currencyPart = currency != null ? $"\\ \"{currency}\"" : string.Empty;

            return style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .NumberFormat.SetFormat($"# ##0.{precisionPart}{currencyPart}");
        }

        private static IXLStyle ApplyTextStyle(IXLStyle style)
        {
            return style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        }
    }
}
