using HulluKyla.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace HulluKyla.Services {
    public static class PdfService {
        public static async Task TulostaLasku(Lasku lasku) {
            if (lasku == null)
                throw new ArgumentNullException(nameof(lasku));

            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 14, XFontStyle.Regular);

            double margin = 40;
            double y = margin;

            gfx.DrawString($"Lasku ID: {lasku.LaskuId}", font, XBrushes.Black, new XRect(margin, y, page.Width, page.Height), XStringFormats.TopLeft);
            y += 30;
            gfx.DrawString($"Varaus ID: {lasku.VarausId}", font, XBrushes.Black, new XRect(margin, y, page.Width, page.Height), XStringFormats.TopLeft);
            y += 30;
            gfx.DrawString($"Summa: {lasku.Summa:f2} €", font, XBrushes.Black, new XRect(margin, y, page.Width, page.Height), XStringFormats.TopLeft);
            y += 30;
            gfx.DrawString($"ALV: {lasku.Alv:f2} €", font, XBrushes.Black, new XRect(margin, y, page.Width, page.Height), XStringFormats.TopLeft);
            y += 30;
            gfx.DrawString($"Maksettu: {(lasku.Maksettu ? "Kyllä" : "Ei")}", font, XBrushes.Black, new XRect(margin, y, page.Width, page.Height), XStringFormats.TopLeft);

            var tiedostonimi = $"Lasku_{lasku.LaskuId}.pdf";
            var tiedostopolku = Path.Combine(FileSystem.Current.AppDataDirectory, tiedostonimi);

            using var stream = File.Create(tiedostopolku);
            document.Save(stream);

            // Näytetään polku käyttäjälle
            await Shell.Current.DisplayAlert("PDF tallennettu", $"Tiedosto tallennettu:\n{tiedostopolku}", "OK");
        }

    }
}

