using HulluKyla.Models;
using HulluKyla.Services;

namespace HulluKyla.Pages {
    public partial class SeurantaPage : ContentPage {
        private List<Lasku> kaikkiLaskut = new();
        private List<Asiakas> kaikkiAsiakkaat = new();
        private List<Asiakas> suodatetutAsiakkaat = new();
        private Dictionary<uint, uint> varausAsiakasMap = new();

        public SeurantaPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            LataaData();
        }

        private void LataaData() {
            // Luo laskut tarvittaessa
            LaskuService.LuoLaskutPaattyneille();

            // Haetaan data
            kaikkiLaskut = LaskuService.HaeKaikki();
            kaikkiAsiakkaat = AsiakasService.HaeKaikki();
            suodatetutAsiakkaat = kaikkiAsiakkaat;

            // Asiakaslistan alustus
            AsiakasListaView.ItemsSource = suodatetutAsiakkaat;

            var varaukset = VarausService.HaeKaikki();

            // Tyhjennetään varmuuden vuoksi ennen uudelleentäyttöä
            varausAsiakasMap.Clear();
            foreach (var varaus in varaukset) {
                varausAsiakasMap[varaus.VarausId] = varaus.Asiakas.AsiakasId;
            }

            // Näytetään kaikki laskut alkuun
            LaskuLista.ItemsSource = kaikkiLaskut;
            TyhjaIlmoitus.IsVisible = kaikkiLaskut.Count == 0;
        }

        private void AsiakasSearchBar_TextChanged(object sender, TextChangedEventArgs e) {
            string haku = AsiakasSearchBar.Text?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(haku)) {
                suodatetutAsiakkaat = kaikkiAsiakkaat;
                AsiakasListaView.SelectedItem = null;
                LaskuLista.ItemsSource = kaikkiLaskut;
                TyhjaIlmoitus.IsVisible = kaikkiLaskut.Count == 0;
            } else {
                suodatetutAsiakkaat = kaikkiAsiakkaat
                    .Where(a =>
                        !string.IsNullOrWhiteSpace(a.KokoNimi) &&
                        a.KokoNimi.ToLower().Contains(haku))
                    .ToList();

                // Nollaa valinta, jos listaa suodatetaan
                AsiakasListaView.SelectedItem = null;
            }

            AsiakasListaView.ItemsSource = suodatetutAsiakkaat;
        }

        private void AsiakasListaView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.CurrentSelection.FirstOrDefault() is Asiakas valittu) {
                PaivitaNakyma(valittu);
            }
        }

        private void MaksamattomatSwitch_Toggled(object sender, ToggledEventArgs e) {
            if (AsiakasListaView.SelectedItem is Asiakas valittu) {
                PaivitaNakyma(valittu);
            } else {
                // Suodatetaan koko lista, ilman asiakasta
                IEnumerable<Lasku> suodatettu = kaikkiLaskut;

                if (MaksamattomatSwitch.IsToggled) {
                    suodatettu = suodatettu.Where(l => !l.Maksettu);
                }

                var lista = suodatettu.ToList();
                LaskuLista.ItemsSource = lista;
                TyhjaIlmoitus.IsVisible = lista.Count == 0;
            }
        }

        private void PaivitaNakyma(Asiakas valittuAsiakas) {
            if (valittuAsiakas == null)
                return;

            IEnumerable<Lasku> suodatettu = kaikkiLaskut;

            suodatettu = suodatettu.Where(l =>
                varausAsiakasMap.TryGetValue(l.VarausId, out uint asiakasId) &&
                asiakasId == valittuAsiakas.AsiakasId);

            if (MaksamattomatSwitch.IsToggled) {
                suodatettu = suodatettu.Where(l => !l.Maksettu);
            }

            var lista = suodatettu.ToList();
            LaskuLista.ItemsSource = lista;
            TyhjaIlmoitus.IsVisible = lista.Count == 0;
        }

        private async void Navigoi_Clicked(object sender, EventArgs e) {
            var reitti = (sender as Button)?.CommandParameter as string;
            if (!string.IsNullOrEmpty(reitti))
                await NavigointiService.Navigoi(reitti);
        }
    }
}
