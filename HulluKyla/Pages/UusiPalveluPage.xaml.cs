using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;

namespace HulluKyla.Pages;

public partial class UusiPalveluPage : ContentPage {
    public UusiPalveluPage() {
        InitializeComponent();
    }

    // Sivun p‰ivitys toiminnot, kun sivulle tullaan
    protected override void OnAppearing() {
        base.OnAppearing();
        TyhjennaKentat();
        AluePicker.ItemsSource = AlueService.HaeKaikki();
    }

    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    // LisaaClicked -metodi
    private async void LisaaClicked(object sender, EventArgs e) {
        try {

            if (AluePicker.SelectedItem is not Alue valittuAlue){
                await DisplayAlert("Virhe", "Valitse ensin alue.", "OK");
                return;
            }


            if (double.TryParse(HintaEntry.Text, out double hinta)) {
                
            } else {
                await DisplayAlert("Virhe", "Virheellinen hinta-arvo", "OK");
                return;
            }
            if (double.TryParse(AlvEntry.Text, out double alv)) {

            } else {
                await DisplayAlert("Virhe", "Virheellinen ALV-arvo", "OK");
                return;
            }

            var uusiPalvelu = new Palvelu(
                valittuAlue.AlueId,
                hinta,
                alv,
                NimiEntry.Text,
                KuvausEntry.Text  
            );

            PalveluService.Lisaa(uusiPalvelu);

            await DisplayAlert("Onnistui", "Palvelu lis‰tty", "OK");
            TyhjennaKentat();
            await NavigointiService.Navigoi("PalveluListaPage");

        }
        catch (Exception ex) {
            await DisplayAlert("Virhe", $"Palvelun lis‰‰misess‰ tapahtui virhe: {ex.Message}", "OK");
        }
        
    }

    // Kenttien tyhjennys
    private void TyhjennaKentat() {
        AluePicker.SelectedItem = null;
        NimiEntry.Text = string.Empty;
        KuvausEntry.Text = string.Empty;
        HintaEntry.Text = string.Empty;
        AlvEntry.Text = string.Empty;
    }

}
