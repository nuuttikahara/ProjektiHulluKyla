using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;
using MySql.Data.MySqlClient;

namespace HulluKyla.Pages;

public partial class MokkiListaPage : ContentPage {
    
    private Mokki? valittuMokki;
    private List<Alue> kaikkiAlueet = new List<Alue>();
    private List<Mokki> kaikkiMokit = new List<Mokki>();
    public MokkiListaPage() {
        InitializeComponent();
    }

    // Sivun päivitystoiminnot kun sivu tulee esiin
    protected override void OnAppearing() {
        base.OnAppearing();
        TyhjennaKentat();
        PaivitaLista();
        LataaAlueet();
    }

    // Navigointi
    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target) {
            await NavigointiService.Navigoi(target);
        }
    }

    // CollectionView-listan päivitys
    private void PaivitaLista() {
        valittuMokki = null;
        kaikkiMokit = MokkiService.HaeKaikki();
        MokkiLista.ItemsSource = kaikkiMokit;
    }

    // Hakee kaikki alueet sivulle tultaessa  
    private void LataaAlueet() {
        AluePicker.SelectedItem = null;
        kaikkiAlueet = AlueService.HaeKaikki();
        AluePicker.ItemsSource = kaikkiAlueet;
    }

    // Näyttää CollectionViewissä vain valitun alueen mökit
    private void AluePickerSelected(object sender, EventArgs e) {

        var valittuAlue = (Alue)AluePicker.SelectedItem;

        if (valittuAlue != null) {
            var mokit = MokkiService.HaeAlueenMokit(valittuAlue.AlueId);
            MokkiLista.ItemsSource = mokit;
        } 
        else {
            MokkiLista.ItemsSource = null;
        }
    }

    // Kenttien tyhjennys
    private void TyhjennaKentat() {
        MokkiNimiEntry.Text = string.Empty;
        KatuosoiteEntry.Text = string.Empty;
        PostiNroEntry.Text = string.Empty;
        HintaEntry.Text = string.Empty;
        KuvausEntry.Text = string.Empty;
        HenkiloMaaraEntry.Text = string.Empty;
        VarusteluEntry.Text = string.Empty;
        valittuMokki = null;
    }

    // Mokki-olion valinta CollectionView-listasta
    private void MokkiSelected(object sender, SelectionChangedEventArgs e) {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0) {
            valittuMokki = e.CurrentSelection[0] as Mokki;

            if (valittuMokki != null) {
                MokkiNimiEntry.Text = valittuMokki.MokkiNimi;
                KatuosoiteEntry.Text = valittuMokki.Katuosoite;
                PostiNroEntry.Text = valittuMokki.Postinro;
                HintaEntry.Text = valittuMokki.Hinta.ToString();
                KuvausEntry.Text = valittuMokki.Kuvaus;
                HenkiloMaaraEntry.Text = valittuMokki.HenkiloMaara.ToString();
                VarusteluEntry.Text = valittuMokki.Varustelu;
            }
        }
    }

    // Mökin tietojen päivittäminen/-ja tallennus 
    private async void TallennaClicked(object sender, EventArgs e) {
        if (valittuMokki == null) {
            await DisplayAlert("Virhe", "Valitse ensin mökki", "OK");
            return;
        }

        try {
            valittuMokki.MokkiNimi = MokkiNimiEntry.Text;
            valittuMokki.Katuosoite = KatuosoiteEntry.Text;
            valittuMokki.Postinro = PostiUtil.PostinroHandler(PostiNroEntry.Text);
            valittuMokki.Hinta = double.Parse(HintaEntry.Text);
            valittuMokki.Kuvaus = KuvausEntry.Text;
            valittuMokki.HenkiloMaara = long.Parse(HenkiloMaaraEntry.Text);
            valittuMokki.Varustelu = VarusteluEntry.Text;

            MokkiService.Paivita(valittuMokki);

            await DisplayAlert("Tallennus", "Tiedot tallennettu onnistuneesti", "OK");
            PaivitaLista();
            TyhjennaKentat();
        } 
        catch (Exception ex) {
            await DisplayAlert("Virhe", "Mökin tallennuksessa tapahtui virhe" + ex.Message, "OK");
        }
    }

    // Mökin poistaminen
    private async void PoistaClicked(object sender, EventArgs e) {
        if (valittuMokki == null) {
            await DisplayAlert("Virhe", "Valitse ensin mökki", "OK");
            return;
        }

        try {
            MokkiService.Poista(valittuMokki.MokkiId);
            await DisplayAlert("Poista", "Mökin poisto onnistui", "OK");
            PaivitaLista();
            TyhjennaKentat();
        } 
        catch (MySqlException sqlEx) {
            if (sqlEx.Number == 1451) {
                await DisplayAlert("Tietokantavirhe", "Mökkiä ei voi poistaa, koska se viittaa muihin tauluihin", "OK");
            }
        } 
        catch (Exception ex) {
            await DisplayAlert("Virhe", "Mökkiä poistaessa tapahtui virhe" + ex.Message, "OK");
        }
    }
}
