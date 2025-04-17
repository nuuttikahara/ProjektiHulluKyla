using Microsoft.Maui.Controls;
using HulluKyla.Services;

namespace HulluKyla.Pages;

public partial class MuokkaaPalveluaPage : ContentPage {
    public MuokkaaPalveluaPage() {
        InitializeComponent();
    }

    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private void Tallenna_Clicked(object sender, EventArgs e) {
        // TODO: P‰ivit‰ valittu palvelu SQL:‰‰n PalveluID:n perusteella
    }

}
