using HulluKyla.Models;
using HulluKyla.Services;
using Microsoft.Maui.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HulluKyla.Pages {
    public partial class AsiakasListaPage : ContentPage {
        private Asiakas? valittuAsiakas;

        public AsiakasListaPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            TyhjennaKentat();
            PaivitaLista();
        }

        private void PaivitaLista() {
            AsiakasLista.ItemsSource = AsiakasService.HaeKaikki();
        }

        private void AsiakasSelected(object sender, SelectionChangedEventArgs e) {
            valittuAsiakas = e.CurrentSelection.FirstOrDefault() as Asiakas;
            if (valittuAsiakas != null) {
                EtunimiEntry.Text = valittuAsiakas.Etunimi;
                SukunimiEntry.Text = valittuAsiakas.Sukunimi;
                LahiosoiteEntry.Text = valittuAsiakas.Lahiosoite;
                PostinroEntry.Text = valittuAsiakas.Postinro;
                EmailEntry.Text = valittuAsiakas.Email;
                PuhelinnroEntry.Text = valittuAsiakas.Puhelinnro;
            }
        }

        private void TyhjennaKentat() {
            EtunimiEntry.Text = "";
            SukunimiEntry.Text = "";
            LahiosoiteEntry.Text = "";
            PostinroEntry.Text = "";
            EmailEntry.Text = "";
            PuhelinnroEntry.Text = "";
            AsiakasSearchBar.Text = "";
            valittuAsiakas = null;
        }

        private async void TallennaClicked(object sender, EventArgs e) {
            if (valittuAsiakas == null) {
                await DisplayAlert("Virhe", "Valitse ensin asiakas.", "OK");
                return;
            }

            try {
                valittuAsiakas.Etunimi = EtunimiEntry.Text;
                valittuAsiakas.Sukunimi = SukunimiEntry.Text;
                valittuAsiakas.Lahiosoite = LahiosoiteEntry.Text;
                valittuAsiakas.Postinro = PostinroEntry.Text;
                valittuAsiakas.Email = EmailEntry.Text;
                valittuAsiakas.Puhelinnro = PuhelinnroEntry.Text;

                AsiakasService.Paivita(valittuAsiakas);
                await DisplayAlert("Tallennettu", "Asiakas tallennettu.", "OK");
                PaivitaLista();
            } catch (Exception ex) {
                await DisplayAlert("Virhe", ex.Message, "OK");
            }
        }

        private async void PoistaClicked(object sender, EventArgs e) {
            if (valittuAsiakas == null) {
                await DisplayAlert("Virhe", "Valitse ensin asiakas.", "OK");
                return;
            }

            try {
                AsiakasService.Poista(valittuAsiakas.AsiakasId);
                await DisplayAlert("Poistettu", "Asiakas poistettu onnistuneesti.", "OK");
                TyhjennaKentat();
                PaivitaLista();
            } catch (MySqlException ex) when (ex.Message.Contains("a foreign key constraint fails")) {
                await DisplayAlert("Ei voida poistaa", "Asiakkaalla on vielä varauksia. Poista varaukset ennen asiakkaan poistoa.", "OK");
            } catch (Exception ex) {
                await DisplayAlert("Virhe", $"Poistossa tapahtui virhe: {ex.Message}", "OK");
            }
        }

        private void HaeAsiakkaatClicked(object sender, EventArgs e) {
            string hakusana = AsiakasSearchBar.Text?.Trim();

            if (!string.IsNullOrWhiteSpace(hakusana))
                AsiakasLista.ItemsSource = AsiakasService.HaeHakusanalla(hakusana);
            else
                AsiakasLista.ItemsSource = AsiakasService.HaeKaikki();
        }

        private async void Navigoi_Clicked(object sender, EventArgs e) {
            if (sender is Button btn && btn.CommandParameter is string target) {
                await NavigointiService.Navigoi(target);
            }
        }
    }
}

