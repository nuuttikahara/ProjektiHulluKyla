using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;

namespace HulluKyla.Pages;

public partial class PalveluListaPage : ContentPage {

    private Palvelu? valittuPalvelu;

    public PalveluListaPage() {
        InitializeComponent();
    }

    // Sivun päivitys toiminnot, kun se tulee näkyviin
    protected override void OnAppearing() {
        base.OnAppearing();

        AluePicker.SelectedItem = null;
        PalveluLista.SelectedItem = null;
        TyhjennaKentat();
        AluePicker.ItemsSource = AlueService.HaeKaikki();
        PaivitaLista();
    }

    // Navigointipainikkeiden metodi
    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target) {
            AluePicker.SelectedItem = null;
            PalveluLista.SelectedItem = null;
            TyhjennaKentat();
            await NavigointiService.Navigoi(target);
        }
    }

    // CollectionView-listan päivitys
    private void PaivitaLista() {
        AluePicker.SelectedItem = null;
        PalveluLista.ItemsSource = PalveluService.HaeKaikki();
    }

    // Entry-kenttien tyhjennys
    private void TyhjennaKentat() {
        NimiEntry.Text = string.Empty;
        KuvausEntry.Text = string.Empty;
        HintaEntry.Text = string.Empty;
        AlvEntry.Text = string.Empty;
    }


    // CollectionViewin suodatus pickerin valinnalla
    private void AluePicker_SelectedIndexChanged(object sender, EventArgs e) {
        if (AluePicker.SelectedItem is Alue valittuAlue) {
            PalveluLista.ItemsSource = PalveluService.HaeAlueenPalvelut(valittuAlue.AlueId);
        }
    }


    // Palvelu-olion valinta CollectionView-listasta
    private void PalveluSelected(object sender, SelectionChangedEventArgs e) {

        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0) {

            valittuPalvelu = e.CurrentSelection[0] as Palvelu;

            if (valittuPalvelu != null) {
                NimiEntry.Text = valittuPalvelu.Nimi;
                KuvausEntry.Text = valittuPalvelu.Kuvaus;
                HintaEntry.Text = valittuPalvelu.Hinta.ToString();
                AlvEntry.Text = valittuPalvelu.Alv.ToString();
            }
        }
    }

    // TallennaClicked -metodi
    private async void TallennaClicked(object sender, EventArgs e) {

        if (valittuPalvelu == null) {
            await DisplayAlert("Virhe", "Valitse ensin palvelu", "OK");
            return;
        }
        valittuPalvelu.Nimi = NimiEntry.Text;
        valittuPalvelu.Kuvaus = KuvausEntry.Text;

        if (double.TryParse(HintaEntry.Text, out double hinta)) {
            valittuPalvelu.Hinta = hinta;
        }
        else {
            await DisplayAlert("Virhe", "Virheellinen hinta-arvo", "OK");
            return;
        }
        if (double.TryParse(AlvEntry.Text, out double alv)) {
            valittuPalvelu.Alv = alv;
        }
        else {
            await DisplayAlert("Virhe", "Virheellinen ALV-arvo", "OK");
            return;
        }
        
        try {
            PalveluService.Paivita(valittuPalvelu);
            await DisplayAlert("Tallenus", "Tiedot tallennettu onnistuneesti", "OK");
            PaivitaLista();
        } 
        catch (Exception ex) {
            await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
        }
    }


    // PoistaClicked -metodi
    private async void PoistaClicked(object sender, EventArgs e) {
        if (valittuPalvelu == null) {
            await DisplayAlert("Virhe", "Valitse ensin palvelu", "OK");
            return;
        }

        try {
            PalveluService.Poista(valittuPalvelu.PalveluId);
            await DisplayAlert("Poista", "Palvelu poistettu onnistuneesti", "OK");
            PaivitaLista();
            TyhjennaKentat();
        }
        catch (Exception ex) {
            await DisplayAlert("Virhe", $"Poistaessa tapahtui virhe: {ex.Message}", "OK");
        }
        
    }


}
