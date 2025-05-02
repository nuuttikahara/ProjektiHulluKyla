using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging.Abstractions;

namespace HulluKyla.Pages;

public partial class VarausListaPage : ContentPage {

    private List<Asiakas> kaikkiAsiakkaat;
    private Asiakas? valittuAsiakas;
    private Varaus? valittuVaraus;
    private VarauksenPalvelu? varauksenPalvelu;
    private Palvelu? lisattavaPalvelu;

    public VarausListaPage() {

        InitializeComponent();

    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        kaikkiAsiakkaat = AsiakasService.HaeKaikki();
        AsiakasListaView.ItemsSource=kaikkiAsiakkaat;
        varauksenPalvelu = null;
        lisattavaPalvelu = null;
        valittuVaraus = null;
        AsiakkaanVaraukset.ItemsSource = null;
        VarauksenPalvelut.ItemsSource = null;
        KaikkiPalvelut.ItemsSource = null;
        LkmEntry.Text = "";
    }

    
    
    private async void AsiakasSearchPressed(object sender, EventArgs e) {

        try {
            var hakusana = AsiakasSearchBar.Text?.Trim();

            if (!string.IsNullOrEmpty(hakusana)) {

                var asiakkaat = AsiakasService.HaeHakusanalla(hakusana);
                AsiakasListaView.ItemsSource = asiakkaat;
            } else {
                kaikkiAsiakkaat = AsiakasService.HaeKaikki();
                AsiakasListaView.ItemsSource = kaikkiAsiakkaat;
            }
        } catch (Exception ex) {
            await DisplayAlert("Virhe", "Asiakkaiden haussa tapahtui virhe" + ex.Message, "OK");
        }

    }
    private async void AsiakasSelected(object sender, SelectionChangedEventArgs e) {
        valittuAsiakas = e.CurrentSelection.FirstOrDefault() as Asiakas;
        AsiakkaanVaraukset.ItemsSource=VarausService.HaeAsiakkaanVaraukset(valittuAsiakas.AsiakasId);
    }

    private async void VarausSelected(object sender, SelectionChangedEventArgs e) {
        valittuVaraus = e.CurrentSelection.FirstOrDefault() as Varaus;
        VarauksenPalvelut.ItemsSource = VarauksenPalvelutService.HaeVarauksenPalvelut(valittuVaraus.VarausId);
        KaikkiPalvelut.ItemsSource = PalveluService.HaeAlueenPalvelut(valittuVaraus.Mokki.AlueId);
    }
    private async void VarauksenPalveluSelected(object sender, SelectionChangedEventArgs e) {
        varauksenPalvelu = e.CurrentSelection.FirstOrDefault() as VarauksenPalvelu;
    }

    private async void KaikkiPalvelutSelected(object sender, SelectionChangedEventArgs e) {
        lisattavaPalvelu = e.CurrentSelection.FirstOrDefault() as Palvelu;
    }

    private async void PoistaVaraus_Clicked(object sender, EventArgs e) {
        if (valittuVaraus == null) {
            await DisplayAlert("Virhe", "Valitse ensin varaus", "OK");
            return;
        }
        try {
            VarausService.Poista(valittuVaraus.VarausId);
            await DisplayAlert("Poista", "Varauksen poisto onnistui", "OK");
            PaivitaSivu();

        } catch (Exception ex) {
            await DisplayAlert("Virhe", $"Varausta poistettaessa tapahtui virhe: {ex.Message}", "OK");

        }
    }

    private async void PoistaPalvelu_Clicked(object sender, EventArgs e) {
        if (valittuVaraus == null || varauksenPalvelu == null) {
            await DisplayAlert("Virhe", "Valitse ensin varaus ja poistettava palvelu", "OK");
            return;
        }
        try {
            VarauksenPalvelutService.PoistaPalveluVarauksesta(valittuVaraus.VarausId, varauksenPalvelu.Palvelu.PalveluId);
            await DisplayAlert("Poista", "Palvelun poisto varauksesta onnistui", "OK");
            PaivitaSivu();

        } catch (Exception ex) {
            await DisplayAlert("Virhe", $"Palvelua poistettaessa tapahtui virhe: {ex.Message}", "OK");
        }
    }

    private async void LisaaPalvelu_Clicked(object sender, EventArgs e) {
        if (valittuVaraus == null || lisattavaPalvelu == null) {
            await DisplayAlert("Virhe", "Valitse ensin varaus ja lis‰tt‰v‰ palvelu", "OK");
            return;
        }
        if (int.TryParse(LkmEntry.Text, out int lkm)) {

        } else {
            await DisplayAlert("Virhe", "Virheellinen kappalem‰‰r‰", "OK");
            return;
        }
        try {
            VarauksenPalvelutService.LisaaPalveluVaraukseen(valittuVaraus.VarausId, lisattavaPalvelu.PalveluId, lkm);
            await DisplayAlert("Lis‰‰", "Palvelun lis‰ys varaukseen onnistui", "OK");
            PaivitaSivu();

        } catch (Exception ex){
            await DisplayAlert("Virhe", $"Palvelua lis‰tt‰ess‰ tapahtui virhe: {ex.Message}", "OK");
        }
    }

    private void PaivitaSivu() {
        varauksenPalvelu = null;
        lisattavaPalvelu = null;
        valittuVaraus = null;
        AsiakkaanVaraukset.ItemsSource = null;
        VarauksenPalvelut.ItemsSource = null;
        KaikkiPalvelut.ItemsSource = null;
        LkmEntry.Text = "";
        AsiakkaanVaraukset.ItemsSource = VarausService.HaeAsiakkaanVaraukset(valittuAsiakas.AsiakasId);
    }


    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private void Tyhja_Clicked(object sender, EventArgs e) {
        // Ei m‰‰ritetty‰ logiikkaa
    }
}
