using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;

namespace HulluKyla.Pages;

public partial class HakutyokaluPage : ContentPage {
    private DateTime hakuAlkaa;
    private DateTime hakuLoppuu;
    private int henkiloMaara = 0;

    public HakutyokaluPage() {
        InitializeComponent();
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        PaivitaSivu();
        HakuPicker.SelectedItem = null;
        MokkiHakuNakyma.IsVisible = false;
        AsiakasHakuNakyma.IsVisible = false;
        PalveluHakuNakyma.IsVisible = false;
    }

    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    // Hakukategorian valinta
    private void HakuSelected(object sender, EventArgs e) {
        PaivitaSivu();
        string valinta = HakuPicker.SelectedItem as string;

        MokkiHakuNakyma.IsVisible = false;
        AsiakasHakuNakyma.IsVisible = false;
        PalveluHakuNakyma.IsVisible = false;

        switch (valinta) {
            case "Mökit":
                MokkiHakuNakyma.IsVisible = true;
                break;
            case "Asiakkaat":
                AsiakasHakuNakyma.IsVisible = true;
                break;
            case "Palvelut":
                PalveluHakuNakyma.IsVisible = true;
                break;
        }
    }

    // Mökkien haku
    private async void HaeMokitClicked(object sender, EventArgs e) {
        if (MokinAluePicker.SelectedItem is not Alue valittuAlue) {
            await DisplayAlert("Virhe", "Valitse ensin alue.", "OK");
            return;
        }

        hakuAlkaa = YhdistaPaivaJaAika(AlkuPvmPicker, AlkuAikaPicker);
        hakuLoppuu = YhdistaPaivaJaAika(LoppuPvmPicker, LoppuAikaPicker);

        if (hakuAlkaa >= hakuLoppuu) {
            await DisplayAlert("Virhe", "Haku ei voi päättyä ennen alkamista.", "OK");
            return;
        }

        try {
            if (int.TryParse(HenkilomaaraEntry.Text, out int maara)) {
                henkiloMaara = maara;
            }

            var tulokset = MokkiService.HaeVapaatMokit(valittuAlue.AlueId, henkiloMaara, hakuAlkaa, hakuLoppuu);

            if (tulokset.Count == 0) {
                await DisplayAlert("Ei tuloksia", "Ei vapaita mökkejä valituilla ehdoilla.", "OK");
                return;
            }

            MokkiTulosLista.ItemsSource = tulokset;
        } 
        catch (Exception ex) {
            await DisplayAlert("Virhe", $"Mökkejä haettaessa tapahtui virhe: {ex.Message}", "OK");
            return;
        }
    }

    // Asiakkaiden SearchBar -haku
    private void HaeAsiakkaatClicked(object sender, EventArgs e) {
        string hakusana = AsiakasSearchBar.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(hakusana))
            AsiakasTulosLista.ItemsSource = AsiakasService.HaeHakusanalla(hakusana);
        else
            AsiakasTulosLista.ItemsSource = AsiakasService.HaeKaikki();
    }

    // Palveluiden haku alueen perusteella
    private void PalvelunAlueSelected(object sender, EventArgs e) {
        if (PalvelunAluePicker.SelectedItem is Alue valittuAlue) {
            PalveluTulosLista.ItemsSource = PalveluService.HaeAlueenPalvelut(valittuAlue.AlueId);
        }
    }

    private void PaivitaSivu() {
        // Pickereiden tyhjennys ja haku
        hakuAlkaa = DateTime.Today;
        hakuLoppuu = DateTime.Today;
        AlkuPvmPicker.Date = DateTime.Today;
        LoppuPvmPicker.Date = DateTime.Today;
        AlkuAikaPicker.Time = new TimeSpan(12, 0, 0);
        LoppuAikaPicker.Time = new TimeSpan(12, 0, 0);

        MokinAluePicker.SelectedItem = null;
        MokinAluePicker.ItemsSource = AlueService.HaeKaikki();
        PalvelunAluePicker.SelectedItem = null;
        PalvelunAluePicker.ItemsSource = AlueService.HaeKaikki();

        // Listojen tyhjennys ja haku;
        MokkiTulosLista.ItemsSource = MokkiService.HaeKaikki();
        AsiakasTulosLista.ItemsSource = AsiakasService.HaeKaikki();
        PalveluTulosLista.ItemsSource = PalveluService.HaeKaikki();

        // Muiden elementtien tyhjennys
        HenkilomaaraEntry.Text = "";
        AsiakasSearchBar.Text = "";
    }

    // Date ja timepicker yhdistysmetodi
    private DateTime YhdistaPaivaJaAika(DatePicker datePicker, TimePicker timePicker) {
        var pvm = datePicker.Date;
        var aika = timePicker.Time;
        return new DateTime(pvm.Year, pvm.Month, pvm.Day, aika.Hours, aika.Minutes, aika.Seconds);
    }

    
}
