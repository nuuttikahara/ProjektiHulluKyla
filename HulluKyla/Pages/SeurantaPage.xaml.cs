using HulluKyla.Models;
using HulluKyla.Services;

namespace HulluKyla.Pages {
    public partial class SeurantaPage : ContentPage {
        private List<Lasku> kaikkiLaskut = new();
        private List<Asiakas> kaikkiAsiakkaat = new();
        private List<Asiakas> suodatetutAsiakkaat = new();
        private Dictionary<uint, uint> varausAsiakasMap = new();
        private bool naytaVainMaksamattomat = true;
        private Lasku valittuLasku;

        public SeurantaPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            LataaData();
        }

        private void LataaData() {
            LaskuService.LuoLaskutPaattyneille();

            kaikkiLaskut = LaskuService.HaeKaikki();
            kaikkiAsiakkaat = AsiakasService.HaeKaikki();
            suodatetutAsiakkaat = kaikkiAsiakkaat;

            AsiakasListaView.ItemsSource = suodatetutAsiakkaat;

            var varaukset = VarausService.HaeKaikki();
            varausAsiakasMap.Clear();
            foreach (var varaus in varaukset) {
                varausAsiakasMap[varaus.VarausId] = varaus.Asiakas.AsiakasId;
            }

            PaivitaNakyma(null);
        }

        private void AsiakasSearchBar_TextChanged(object sender, TextChangedEventArgs e) {
            string haku = AsiakasSearchBar.Text?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(haku)) {
                suodatetutAsiakkaat = kaikkiAsiakkaat;
                AsiakasListaView.SelectedItem = null;
                PaivitaNakyma(null);
            } else {
                suodatetutAsiakkaat = kaikkiAsiakkaat
                    .Where(a => a.KokoNimi.ToLower().Contains(haku))
                    .ToList();

                AsiakasListaView.SelectedItem = null;
            }

            AsiakasListaView.ItemsSource = suodatetutAsiakkaat;
        }

        private void AsiakasListaView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.CurrentSelection.FirstOrDefault() is Asiakas valittu) {
                PaivitaNakyma(valittu);
            }
        }

        private void VaihdaNakyma_Clicked(object sender, EventArgs e) {
            naytaVainMaksamattomat = !naytaVainMaksamattomat;
            VaihdaNakymaButton.Text = naytaVainMaksamattomat
                ? "Maksamattomat laskut"
                : "Kaikki laskut";

            PaivitaNakyma(AsiakasListaView.SelectedItem as Asiakas);
        }

        private void PaivitaNakyma(Asiakas valittuAsiakas) {
            IEnumerable<Lasku> suodatettu = kaikkiLaskut;

            if (valittuAsiakas != null) {
                suodatettu = suodatettu.Where(l =>
                    varausAsiakasMap.TryGetValue(l.VarausId, out uint asiakasId) &&
                    asiakasId == valittuAsiakas.AsiakasId);
            }

            if (naytaVainMaksamattomat) {
                suodatettu = suodatettu.Where(l => !l.Maksettu);
            }

            var lista = suodatettu.ToList();
            LaskuLista.ItemsSource = lista;
            TyhjaIlmoitus.IsVisible = lista.Count == 0;
        }

        private void LaskuLista_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            valittuLasku = e.CurrentSelection.FirstOrDefault() as Lasku;
        }

        private async void TulostaLasku_Clicked(object sender, EventArgs e) {
            if (valittuLasku != null) {
                try {
                    if (varausAsiakasMap.TryGetValue(valittuLasku.VarausId, out uint asiakasId)) {
                        var asiakas = kaikkiAsiakkaat.FirstOrDefault(a => a.AsiakasId == asiakasId);

                        if (asiakas != null) {
                            await PdfService.TulostaLasku(valittuLasku, asiakas);
                        } else {
                            await DisplayAlert("Virhe", "Asiakasta ei löytynyt.", "OK");
                        }
                    } else {
                        await DisplayAlert("Virhe", "Varaus-ID ei täsmää.", "OK");
                    }
                } catch (Exception ex) {
                    await DisplayAlert("Virhe", $"PDF:n luonti epäonnistui: {ex.Message}", "OK");
                }
            } else {
                await DisplayAlert("Ei laskua", "Valitse lasku ennen tulostamista.", "OK");
            }
        }


        private async void Navigoi_Clicked(object sender, EventArgs e) {
            var reitti = (sender as Button)?.CommandParameter as string;
            if (!string.IsNullOrEmpty(reitti))
                await NavigointiService.Navigoi(reitti);
        }
    }
}



