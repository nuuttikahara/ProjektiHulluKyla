using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;
using System.Threading.Tasks;

namespace HulluKyla.Pages;

public partial class UusiVarausPage : ContentPage {

    private Alue? valittuAlue;
    private Mokki? valittuMokki;
    private Asiakas? valittuAsiakas;
    private DateTime varausAlkaa;
    private DateTime varausLoppuu;
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
        varausAlkaa = DateTime.Today;
        YhteydenottoPvmPicker.Date = DateTime.Today;
        AlkuPvmPicker.Date = DateTime.Today;
        LoppuPvmPicker.Date = DateTime.Today;

        varausLoppuu = DateTime.Today;
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
            
            varausAlkaa = YhdistaPaivaJaAika(AlkuPvmPicker, AlkuAikaPicker);
            varausLoppuu = YhdistaPaivaJaAika(LoppuPvmPicker, LoppuAikaPicker);

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

    // Varauksen lisäys-metodi
    private async void LisaaVaraus_Clicked(object sender, EventArgs e) {
        varausAlkaa = YhdistaPaivaJaAika(AlkuPvmPicker, AlkuAikaPicker);
        varausLoppuu = YhdistaPaivaJaAika(LoppuPvmPicker, LoppuAikaPicker);

        if (varausLoppuu <= varausAlkaa) {
            await DisplayAlert("Virhe", "Varaus ei voi päättyä ennen kuin se alkaa.", "OK");
            return;
        }

        if (valittuAsiakas == null) {
            await DisplayAlert("Virhe", "Valitse asiakas", "OK");
            return;
        }

        if (valittuMokki == null) {
            await DisplayAlert("Virhe", "Valitse mökki", "OK");
            return;
        }

        DateTime tilausPvm = YhdistaPaivaJaAika(YhteydenottoPvmPicker, YhteydenottoAikaPicker);

        if (tilausPvm.Date >= varausAlkaa.Date && tilausPvm.TimeOfDay >= varausAlkaa.TimeOfDay) {
            await DisplayAlert("Virhe", "Tilausaika täytyy olla ennen varausta.", "OK");
            return;
        }

        try {
            var uusiVaraus = new Varaus(
                valittuAsiakas,
                valittuMokki,
                tilausPvm,
                DateTime.Now,
                varausAlkaa,
                varausLoppuu);

            VarausService.Lisaa(uusiVaraus);

            await DisplayAlert("Onnistui", "Varaus lisätty", "OK");
            await NavigointiService.Navigoi("VarausListaPage");
        }
        catch (Exception ex) {
            await DisplayAlert("Virhe", "Varausta lisättäessä tapahtui virhe: " + ex.Message, "OK");
            return;
        }
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
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            valittuMokki = e.CurrentSelection.FirstOrDefault() as Mokki;
    }
    
    // Asiakkaan valinta CollectionView-listasta
    private void AsiakasSelected(object sender, SelectionChangedEventArgs e) {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            valittuAsiakas = e.CurrentSelection.FirstOrDefault() as Asiakas;
    }
}
