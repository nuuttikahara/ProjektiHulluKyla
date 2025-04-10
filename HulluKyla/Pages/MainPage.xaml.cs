using HulluKyla.Pages;
namespace HulluKyla.Pages;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }

    private async void TestiSivuClicked(object sender, EventArgs e) 
    {
        await Navigation.PushAsync(new TestiSivu());
    }

}



