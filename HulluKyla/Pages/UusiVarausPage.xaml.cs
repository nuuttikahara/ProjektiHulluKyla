using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;
using System.Threading.Tasks;

namespace HulluKyla.Pages;

public partial class UusiVarausPage : ContentPage {

    private Alue? valittuAlue;
    private Mokki? valittuMokki;
    private Asiakas? valittuAsiakas;
    public UusiVarausPage() {
        InitializeComponent();
    }

    // Sivun päivitystoiminnot kun sivu tulee esiin
    protected override void OnAppearing() {
        base.OnAppearing();
        PaivitaSivu();
        LataaAlueet();
    }

    // Päivittää koko sivun elementit
    private void PaivitaSivu() {
        // Listojen tyhjennys ja haku
        valittuMokki = null;
        var kaikkiMokit = MokkiService.HaeKaikki();
        MokkiLista.ItemsSource = kaikkiMokit;
        
        valittuAsiakas = null;
        var kaikkiAsiakkaat = AsiakasService.HaeKaikki();
        AsiakasLista.ItemsSource = kaikkiAsiakkaat;

        // Pickereiden tyhjennys
        YhteydenottoPvmPicker.Date = DateTime.Today;
        AlkuPvmPicker.Date = DateTime.Today;
        LoppuPvmPicker.Date = DateTime.Today;

        YhteydenottoAikaPicker.Time = new TimeSpan(12, 0, 0);
        AlkuAikaPicker.Time = new TimeSpan(12, 0, 0);
        LoppuAikaPicker.Time = new TimeSpan(12, 0, 0);

        // Muiden elementtien tyhjennys
        AsiakasSearchBar.Text = string.Empty;
        AluePicker.SelectedItem = null;
        HenkiloMaaraEntry.Text = string.Empty;
    }

    // Hakee kaikki alueet
    private void LataaAlueet() {
        var kaikkiAlueet = AlueService.HaeKaikki();
        AluePicker.ItemsSource = kaikkiAlueet; 
    }

    // Date ja timepicker yhdistysmetodi
    private DateTime YhdistaPaivaJaAika(DatePicker datePicker, TimePicker timePicker) {
        var pvm = datePicker.Date;
        var aika = timePicker.Time;
        return new DateTime(pvm.Year, pvm.Month, pvm.Day, aika.Hours, aika.Minutes, aika.Seconds);
    }

    // Mökkien haku-metodi
    private async void MokkiHaku_Clicked(object sender, EventArgs e) {

        try {
            if (AluePicker.SelectedItem == null) {
                await DisplayAlert("Virhe", "Valitse alue", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(HenkiloMaaraEntry.Text) || !int.TryParse(HenkiloMaaraEntry.Text, out int minHenkilomaara)) {
                await DisplayAlert("Virhe", "Syötä kelvollinen henkilömäärä.", "OK");
                return;
            }

            valittuAlue = (Alue)AluePicker.SelectedItem;
            uint alueId = valittuAlue.AlueId;
            
            DateTime varausAlkaa = YhdistaPaivaJaAika(AlkuPvmPicker, AlkuAikaPicker);
            DateTime varausLoppuu = YhdistaPaivaJaAika(LoppuPvmPicker, LoppuAikaPicker);

            if (varausLoppuu <= varausAlkaa) {
                await DisplayAlert("Virhe", "Varaus ei voi päättyä ennen kuin se alkaa.", "OK");
                return;
            }

            var vapaatMokit = MokkiService.HaeVapaatMokit(alueId, minHenkilomaara, varausAlkaa, varausLoppuu);

            if (vapaatMokit.Count == 0) {
                await DisplayAlert("Ei mökkejä", "Hakuehdoilla ei löytynyt vapaita mökkejä.", "OK");
                MokkiLista.ItemsSource = null;
            } else {
                MokkiLista.ItemsSource = vapaatMokit;
            }
        }
        catch (Exception ex) {
            await DisplayAlert("Virhe", "Mökkien hakemisessa tapahtui virhe: " + ex.Message, "OK");
        }
    }
    
    // Navigointi
    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private void TallennaVaraus_Clicked(object sender, EventArgs e) {
         
    }

    // Asiakkaan haku-metodi
    private async void AsiakasSearchPressed(object sender, EventArgs e) {
        try {
            var hakusana = AsiakasSearchBar.Text?.Trim();

            if (!string.IsNullOrEmpty(hakusana)) {

                var asiakkaat = AsiakasService.HaeHakusanalla(hakusana);
                AsiakasLista.ItemsSource = asiakkaat;
            } else {
                var kaikkiAsiakkaat = AsiakasService.HaeKaikki();
                AsiakasLista.ItemsSource = kaikkiAsiakkaat;
            }
        } 
        catch (Exception ex) {
            await DisplayAlert("Virhe", "Asiakkaiden haussa tapahtui virhe" + ex.Message, "OK");
        } 
    }

    // Mokki-olion valinta CollectionView-listasta
    private void MokkiSelected(object sender, SelectionChangedEventArgs e) {
        valittuMokki = e.CurrentSelection.FirstOrDefault() as Mokki;
    }
    
    // Asiakkaan valinta CollectionView-listasta
    private void AsiakasSelected(object sender, SelectionChangedEventArgs e) {
        valittuAsiakas = e.CurrentSelection.FirstOrDefault() as Asiakas;
    }
}
