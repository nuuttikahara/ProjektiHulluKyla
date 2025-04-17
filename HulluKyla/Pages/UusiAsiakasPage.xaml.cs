using Microsoft.Maui.Controls;
using HulluKyla.Services;

namespace HulluKyla.Pages;

public partial class UusiAsiakasPage : ContentPage {
    public UusiAsiakasPage() {
        InitializeComponent();
    }

    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private void Tallenna_Clicked(object sender, EventArgs e) {
        // TODO: Lis‰‰ tallennuslogiikka AsiakasClassin perusteella SQL:‰‰n
    }

}
