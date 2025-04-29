using Microsoft.Maui.Controls;
using HulluKyla.Services;
using HulluKyla.Models;

namespace HulluKyla.Pages;

public partial class UusiMokkiPage : ContentPage {
    
    private List<Alue> kaikkiAlueet = new List<Alue>();
    public UusiMokkiPage() {
        InitializeComponent();
    }

    // Sivun päivitystoiminnot kun sivu tulee esiin
    protected override void OnAppearing() {
        base.OnAppearing();
        TyhjennaKentat();
        LataaAlueet();
    }

    // Valitaan tietty alue, johon syötetään uuden mökin tiedot
    private void AluePickerSelected(object sender, EventArgs e) {
        var valittuAlue = (Alue)AluePicker.SelectedItem;
    }

    // Navigointi
    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target) {
            await NavigointiService.Navigoi(target);
        }
    }

    // Hakee kaikki olemassa olevat alueet sivulle tultaessa
    private void LataaAlueet() {
        AluePicker.SelectedItem = null;
        kaikkiAlueet = AlueService.HaeKaikki();
        AluePicker.ItemsSource = kaikkiAlueet;
    }

    // Uuden mökin lisääminen
    private async void LisaaClicked(object sender, EventArgs e) {
        try {
            var valittuAlue = (Alue)AluePicker.SelectedItem;
            if (valittuAlue == null) {
                await DisplayAlert("Virhe", "Valitse alue", "OK");
                return;
            }

            string postinro = PostiUtil.PostinroHandler(PostiNroEntry.Text);
            double hinta = double.Parse(HintaEntry.Text);
            long henkiloMaara = long.Parse(HloMaaraEntry.Text);

            var uusiMokki = new Mokki(
                valittuAlue.AlueId,
                postinro,
                NimiEntry.Text,
                OsoiteEntry.Text,
                hinta,
                KuvausEntry.Text,
                henkiloMaara,
                VarusteluEntry.Text
                );

            MokkiService.Lisaa(uusiMokki);

            await DisplayAlert("Onnistui", "Mökki lisätty", "OK");
            TyhjennaKentat();
            await NavigointiService.Navigoi("MokkiListaPage");
        } 
        catch (Exception ex) {
            await DisplayAlert("Virhe", "Mökkiä lisättäessä tapahtui virhe:" + ex.Message, "OK");
        }
    }

    // Kenttien tyhjennys
    private void TyhjennaKentat() {
        PostiNroEntry.Text = string.Empty;
        NimiEntry.Text = string.Empty;
        OsoiteEntry.Text = string.Empty;
        HintaEntry.Text = string.Empty;
        KuvausEntry.Text = string.Empty;
        HloMaaraEntry.Text = string.Empty;
        VarusteluEntry.Text = string.Empty;
    }
}
