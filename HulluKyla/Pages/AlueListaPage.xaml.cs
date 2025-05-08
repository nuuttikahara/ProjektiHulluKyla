using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;

namespace HulluKyla.Pages;

public partial class AlueListaPage : ContentPage {

    private Alue? valittuAlue;

    public AlueListaPage() {
        InitializeComponent();
    }

    // Sivun päivitystoiminnot kun sivu tulee esiin
    protected override void OnAppearing() {
        base.OnAppearing();

        TyhjennaKentat();
        PaivitaLista();
    }

    // Navigointi-metodi
    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    // CollectionView-listan päivitys
    private void PaivitaLista() {
       valittuAlue = null;
       AlueLista.ItemsSource = null;
       AlueLista.ItemsSource = AlueService.HaeKaikki();
    }

    // Kenttien tyhjennys
    private void TyhjennaKentat() {
        NimiEntry.Text = string.Empty;
    }

    // Alue-olion valinta CollectionView-listasta
    private void AlueSelected(object sender, SelectionChangedEventArgs e) {

        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0) {

            valittuAlue = e.CurrentSelection[0] as Alue;

            if (valittuAlue != null) {
                NimiEntry.Text = valittuAlue.Nimi;
            }
        }
    }

    private void HaeAlueetClicked(object sender, EventArgs e) {

        string hakusana = AlueSearchBar.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(hakusana)) {
            AlueLista.ItemsSource = AlueService.HaeHakusanalla(hakusana);
        } else {
            AlueLista.ItemsSource = AlueService.HaeKaikki();
        }

    } 

    // Tallennus-metodi
    private async void TallennaClicked(object sender, EventArgs e) {
        if (valittuAlue == null) {
            await DisplayAlert("Virhe", "Valitse ensin alue.", "OK");
            return;
        }

        valittuAlue.Nimi = NimiEntry.Text;

        try {
            AlueService.Paivita(valittuAlue);
            await DisplayAlert("Tallenus", "Tiedot tallennettu onnistuneesti.", "OK");
            PaivitaLista();
            TyhjennaKentat();
        }
        catch (Exception ex) {
            await DisplayAlert("Virhe", $"Alueen tallennuksessa tapahtui virhe: {ex.Message}", "OK");
        }
    }

    // Poistamis-metodi
    private async void PoistaClicked(object sender, EventArgs e) {
        if (valittuAlue == null) {
            await DisplayAlert("Virhe", "Valitse ensin alue.", "OK");
            return;
        }

        try {
            AlueService.Poista(valittuAlue.AlueId);
            await DisplayAlert("Poista", "Alueen poisto onnistui.", "OK");
            PaivitaLista();
            TyhjennaKentat();
        }
        catch (Exception ex) {
            await DisplayAlert("Virhe", $"Aluetta poistaessa tapahtui virhe: {ex.Message}", "OK");
        }
    }
}
