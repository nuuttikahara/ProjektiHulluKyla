using Microsoft.Maui.Controls;
using HulluKyla.Services;

namespace HulluKyla.Pages;

public partial class MuokkaaAsiakastaPage : ContentPage {
    public MuokkaaAsiakastaPage() {
        InitializeComponent();
    }

    private async void Navigoi_Clicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.CommandParameter is string target)
            await NavigointiService.Navigoi(target);
    }

    private void Tallenna_Clicked(object sender, EventArgs e) {
        // TODO: Tallenna muutokset SQL:‰‰n
        // Entryt ladataan ennen n‰kym‰n avautumista esim. OnAppearing() tai constructor
    }

}
