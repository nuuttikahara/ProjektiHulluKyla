using Microsoft.Maui.Controls;
using HulluKyla.Services;

namespace HulluKyla.Pages;

public partial class HakutyokaluPage : ContentPage {
    public HakutyokaluPage() {
        InitializeComponent();
    }

    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private void Tyhja_Clicked(object sender, EventArgs e) {
        // Ei m‰‰ritetty‰ logiikkaa
    }

    private void HakukategoriaSelected(object sender, EventArgs e) {

    }
}
