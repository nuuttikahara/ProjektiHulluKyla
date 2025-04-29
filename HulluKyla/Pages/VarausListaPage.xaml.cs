using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;
using System.Collections.ObjectModel;

namespace HulluKyla.Pages;

public partial class VarausListaPage : ContentPage {

    private List<Asiakas> kaikkiAsiakkaat;
    private Asiakas? valittuAsiakas;
    private Varaus? valittuVaraus;

    public VarausListaPage() {

        InitializeComponent();

    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        kaikkiAsiakkaat = AsiakasService.HaeKaikki();
        AsiakasListaView.ItemsSource=kaikkiAsiakkaat;
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
    }
    private async void VarauksenPalveluSelected(object sender, SelectionChangedEventArgs e) {

    }


    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private void Tyhja_Clicked(object sender, EventArgs e) {
        // Ei m‰‰ritetty‰ logiikkaa
    }
}
