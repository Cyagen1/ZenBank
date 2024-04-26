using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ZenReporting.Contracts;

namespace ZenReporting.Services
{
    public class PdfService : IPdfService
    {
        public MemoryStream CreatePdf(Report report)
        {
            var stream = new MemoryStream();

            using var writer = new PdfWriter(stream);
            writer.SetCloseStream(false);

            using var pdf = new PdfDocument(writer);
            
            var document = new Document(pdf);

            document.Add(new Paragraph($"{report.User.Name} - {report.User.Email}")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12));

            document.Add(new Paragraph($"Your daily account statement for {report.Date.Date}"));

            var table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            table.AddHeaderCell(new Cell().Add(new Paragraph("Amount")));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Currency")));

            foreach (var transaction in report.Transactions)
            {
                table.AddCell(new Cell().Add(new Paragraph($"{transaction.Amount}")));
                table.AddCell(new Cell().Add(new Paragraph($"{transaction.Currency}")));
            }
            table.AddFooterCell(new Cell().Add(new Paragraph($"TOTAL: {report.TransactionsSum}")));

            document.Add(table);

            document.Close();

            stream.Position = 0;
            return stream;

        }
    }
}
