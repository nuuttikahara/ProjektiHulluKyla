using Microsoft.Maui.Controls;
using HulluKyla.Services;

namespace HulluKyla.Pages;

public partial class AlueListaPage : ContentPage {
    public AlueListaPage() {
        InitializeComponent();
    }

    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private void Tyhja_Clicked(object sender, EventArgs e) {
        // Ei m‰‰ritetty‰ logiikkaa
    }
}
