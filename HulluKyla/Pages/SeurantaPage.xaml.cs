using HulluKyla.Models;
using HulluKyla.Services;

namespace HulluKyla.Pages {
    public partial class SeurantaPage : ContentPage {
        private List<Lasku> kaikkiLaskut = new();
        private List<Asiakas> kaikkiAsiakkaat = new();
        private Dictionary<uint, uint> varausAsiakasMap = new();

        public SeurantaPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            LataaData();
        }

        private void LataaData() {
            // Luodaan ensin kaikki tarvittavat laskut (päättyneet varaukset)
            LaskuService.LuoLaskutPaattyneille();

            // Haetaan nyt kaikki laskut
            kaikkiLaskut = LaskuService.HaeKaikki();

            // Haetaan asiakkaat Pickeriin
            kaikkiAsiakkaat = AsiakasService.HaeKaikki();
            AsiakasPicker.ItemsSource = kaikkiAsiakkaat;

            // Haetaan varaukset ja yhdistetään ne asiakasId:hin nopeaa hakua varten
            var varaukset = VarausService.HaeKaikki();
            varausAsiakasMap = varaukset.ToDictionary(v => v.VarausId, v => v.Asiakas.AsiakasId);

            PaivitaNakyma();
        }

        private void AsiakasPicker_SelectedIndexChanged(object sender, EventArgs e) {
            PaivitaNakyma();
        }

        private void MaksamattomatSwitch_Toggled(object sender, ToggledEventArgs e) {
            PaivitaNakyma();
        }

        private void PaivitaNakyma() {
            IEnumerable<Lasku> suodatettu = kaikkiLaskut;

            if (AsiakasPicker.SelectedIndex >= 0) {
                var valittuAsiakas = (Asiakas)AsiakasPicker.SelectedItem;

                suodatettu = suodatettu.Where(l =>
                    varausAsiakasMap.TryGetValue(l.VarausId, out uint asiakasId) &&
                    asiakasId == valittuAsiakas.AsiakasId);
            }

            if (MaksamattomatSwitch.IsToggled) {
                suodatettu = suodatettu.Where(l => !l.Maksettu);
            }

            LaskuLista.ItemsSource = suodatettu.ToList();
        }

        private async void Navigoi_Clicked(object sender, EventArgs e) {
            var reitti = (sender as Button)?.CommandParameter as string;
            if (!string.IsNullOrEmpty(reitti))
                await NavigointiService.Navigoi(reitti);
        }
    }
}
