using Microsoft.Maui.Controls;
using HulluKyla.Services;


namespace HulluKyla.Pages;

public partial class HallintaPage : ContentPage {
    // Admin ikkunan salasana
    private const string salasana = "admin123";

    public HallintaPage() {
        InitializeComponent();
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        PaivitaSivu();
    }

    private void PaivitaSivu() {
        AdminFrame.IsVisible = false;
        SalasanaEntry.Text = "";
    }

    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private void Admin_Clicked(object sender, EventArgs e) {
        AdminFrame.IsVisible = !AdminFrame.IsVisible;
    }

    private async void Tyhjenna_Clicked(object sender, EventArgs e) {
        if (SalasanaEntry.Text?.Trim() != salasana) {
            await DisplayAlert("Virheellinen salasana", "Salasana on väärä. Yritä uudelleen.", "OK");
            return;
        }

        bool vahvistus = await DisplayAlert("Vahvista", "Haluatko varmasti poistaa maksetut laskut ja niihin liittyvät varaukset ja asiakkaat?", "Kyllä", "Peruuta");
        if (!vahvistus) {
            PaivitaSivu();
            return;
        }

        try {
            var maksetutLaskut = LaskuService.HaeMaksetutLaskut();
            var maksamattomatLaskut = LaskuService.HaeMaksamattomatLaskut();
            var maksamattomatVarausIdt = maksamattomatLaskut.Select(l => l.VarausId).ToHashSet();
            var kaikkiVaraukset = VarausService.HaeKaikki().ToDictionary(v => v.VarausId);

            foreach (var lasku in maksetutLaskut) {
                if (!kaikkiVaraukset.TryGetValue(lasku.VarausId, out var varaus)) {
                    continue;
                }

                uint varausId = varaus.VarausId;
                uint asiakasId = varaus.Asiakas.AsiakasId;

                // Poistetaan varauksen palvelut ensin
                VarauksenPalvelutService.PoistaKaikkiPalvelutVaraukselta(varausId);

                // Poistetaan lasku varaukselta
                LaskuService.Poista(lasku.LaskuId);

                // Poistetaan varaus
                VarausService.Poista(varausId);

                // Tarkistetaan onko asiakkaalla muita aktiivisia laskuja tai varauksia
                bool onAktiivisia = kaikkiVaraukset.Values.Any(v =>
                    v.Asiakas.AsiakasId == asiakasId &&
                    v.VarausId != varausId &&
                    maksamattomatVarausIdt.Contains(v.VarausId));

                if (!onAktiivisia) {
                    AsiakasService.Poista(asiakasId);
                }

                AsiakasService.PoistaIlmanVarauksia();
            }

            await DisplayAlert("Valmis", "Tyhjennys suoritettu.", "OK");
            PaivitaSivu();
        }
        catch (Exception ex) {
            await DisplayAlert("Virhe", $"Tyhjennys epäonnistui: {ex.Message}", "OK");
        }
        
    }
}
