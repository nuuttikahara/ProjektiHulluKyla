using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;

namespace HulluKyla.Pages;

public partial class UusiAsiakasPage : ContentPage {
    public UusiAsiakasPage() {
        InitializeComponent();
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        TyhjennaKentat();
    }

    private void TyhjennaKentat() {
        EtunimiEntry.Text = string.Empty;
        SukunimiEntry.Text = string.Empty;
        LahiosoiteEntry.Text = string.Empty;
        PostinroEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;
        PuhelinnroEntry.Text = string.Empty;
    }

    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private async void Tallenna_Clicked(object sender, EventArgs e) {
        try {
            var uusiAsiakas = new Asiakas(
                EtunimiEntry.Text,
                SukunimiEntry.Text,
                LahiosoiteEntry.Text,
                PostinroEntry.Text,
                EmailEntry.Text,
                PuhelinnroEntry.Text
            );

            AsiakasService.Lisaa(uusiAsiakas);

            await DisplayAlert("Onnistui", "Uusi asiakas lisätty onnistuneesti.", "OK");

            await NavigointiService.Navigoi("AsiakasListaPage");
        } catch (Exception ex) {
            await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
        }
    }
}

