using HulluKyla.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace HulluKyla.Services {
    public static class PdfService {
        public static async Task TulostaLasku(Lasku lasku, Asiakas asiakas) {
            if (lasku == null)
                throw new ArgumentNullException(nameof(lasku));
            if (asiakas == null)
                throw new ArgumentNullException(nameof(asiakas));

            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Fontit
            var fontRegular = new XFont("Arial", 11, XFontStyle.Regular);
            var fontBold = new XFont("Arial", 11, XFontStyle.Bold);
            var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);

            double margin = 40;
            double y = margin;

            // Yrityksen tiedot (vasen yläkulma)
            gfx.DrawString("HulluKyla Oy", fontBold, XBrushes.Black, margin, y);
            y += 20;
            gfx.DrawString("Hullukyläntie 1", fontRegular, XBrushes.Black, margin, y);
            y += 15;
            gfx.DrawString("99999 Kylä", fontRegular, XBrushes.Black, margin, y);
            y += 15;
            gfx.DrawString("Y-tunnus: 1234567-8", fontRegular, XBrushes.Black, margin, y);

            // Laskun otsikko (keskitetty)
            y = margin + 10;
            gfx.DrawString("LASKU", fontTitle, XBrushes.Black,
                new XRect(0, y, page.Width, 40), XStringFormats.TopCenter);
            y += 60;

            // Asiakkaan tiedot
            gfx.DrawString("Laskun saaja:", fontBold, XBrushes.Black, margin, y);
            y += 20;
            gfx.DrawString(asiakas.KokoNimi, fontRegular, XBrushes.Black, margin, y);
            y += 15;
            gfx.DrawString(asiakas.Lahiosoite, fontRegular, XBrushes.Black, margin, y);
            y += 15;
            gfx.DrawString(asiakas.Postinro, fontRegular, XBrushes.Black, margin, y);
            y += 25;

            // Viiva
            gfx.DrawLine(XPens.Black, margin, y, page.Width - margin, y);
            y += 15;

            // Laskun tiedot
            gfx.DrawString("Laskun tiedot:", fontBold, XBrushes.Black, margin, y);
            y += 20;
            gfx.DrawString($"Lasku ID: {lasku.LaskuId}", fontRegular, XBrushes.Black, margin, y);
            y += 15;
            gfx.DrawString($"Varaus ID: {lasku.VarausId}", fontRegular, XBrushes.Black, margin, y);
            y += 15;
            gfx.DrawString($"Maksettu: {(lasku.Maksettu ? "Kyllä" : "Ei")}", fontRegular, XBrushes.Black, margin, y);
            y += 25;

            // Viiva
            gfx.DrawLine(XPens.Black, margin, y, page.Width - margin, y);
            y += 15;

            // Yhteenveto
            gfx.DrawString("Yhteenveto:", fontBold, XBrushes.Black, margin, y);
            y += 20;
            gfx.DrawString($"Summa yhteensä: {lasku.Summa:f2} €", fontRegular, XBrushes.Black, margin, y);
            y += 15;
            gfx.DrawString($"Sisältää ALV: {lasku.Alv:f2} €", fontRegular, XBrushes.Black, margin, y);
            y += 25;

            // Maksutiedot
            gfx.DrawLine(XPens.Black, margin, y, page.Width - margin, y);
            y += 15;
            gfx.DrawString("Maksuohjeet:", fontBold, XBrushes.Black, margin, y);
            y += 20;
            gfx.DrawString("Saaja: HulluKyla Oy", fontRegular, XBrushes.Black, margin, y);
            y += 15;
            gfx.DrawString("Tilinumero: FI00 1234 5600 0007 89", fontRegular, XBrushes.Black, margin, y);
            y += 15;
            gfx.DrawString($"Viitenumero: LASKU{lasku.LaskuId}", fontRegular, XBrushes.Black, margin, y);
            y += 25;

            // Tallennus
            var tiedostonimi = $"Lasku_{lasku.LaskuId}.pdf";
            var tiedostopolku = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                tiedostonimi
            );

            using var stream = File.Create(tiedostopolku);
            document.Save(stream);

            await Shell.Current.DisplayAlert("PDF tallennettu", $"Tiedosto tallennettu:\n{tiedostopolku}", "OK");
        }
    }
}





