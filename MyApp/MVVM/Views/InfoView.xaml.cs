using MyApp.MVVM.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyApp.MVVM.Views;

public partial class InfoView : ContentPage
{
    public InfoView()
    {
        InitializeComponent();
        BindingContext = new InfoViewModel();
        NavigationPage.SetHasNavigationBar(this, false);
    }
    private async void OnSendButtonClicked(object sender, EventArgs e)
    {
        var subject = "Feedback";
        var body = feedbackEditor.Text; // Assuming feedbackEditor is the Editor control for user input

        var uri = new Uri($"mailto:bdictdictionary@gmail.com?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}");

        try
        {
            await Launcher.OpenAsync(uri);
            await DisplayAlert("Success", "Feedback sent successfully!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}