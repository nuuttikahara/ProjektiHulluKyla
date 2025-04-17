using Microsoft.Maui.Controls;
using HulluKyla.Services;

namespace HulluKyla.Pages;

public partial class UusiVarausPage : ContentPage {
    public UusiVarausPage() {
        InitializeComponent();
    }

    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private void Tallenna_Clicked(object sender, EventArgs e) {
        // TODO: Tallenna uusi palvelu SQL:‰‰n k‰ytt‰en PalveluClassin tietoja
    }
}
