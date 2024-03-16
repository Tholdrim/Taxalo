namespace Taxalo.Integrations.Nbp
{
    internal class NbpXlsxRecord
    {
        [XLCustomColumn("Data", Format = CellFormat.Date, Width = 12.5)]
        public required DateOnly Date { get; init; }

        [XLCustomColumn("Numer tabeli", Width = 25.0)]
        public required string Table { get; init; }

        [XLCustomColumn("Kurs średni", Format = CellFormat.Decimal, Width = 17.5)]
        public required decimal Value { get; init; }
    }
}
