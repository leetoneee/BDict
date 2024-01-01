using MetroLog.Maui;
using Microsoft.Extensions.Logging;
using MyApp.MVVM.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Input;
using Version = MyApp.MVVM.Models.Version;

namespace MyApp.MVVM.Views;

public partial class InfoView : ContentPage
{
    public InfoView()
    {
        InitializeComponent();
        BindingContext = new LogController();
        NavigationPage.SetHasNavigationBar(this, false);
        verNow.Text = AppInfo.VersionString;
    }
    private async void OnSendButtonClicked(object sender, EventArgs e)
    {
        var subject = "Feedback";
        var body = string.Empty;
#if WINDOWS
        body = feedbackEditorW.Text; // Assuming feedbackEditor is the Editor control for user input
#endif

#if ANDROID
        body = feedbackEditorA.Text; // Assuming feedbackEditor is the Editor control for user input
#endif
        if (string.IsNullOrWhiteSpace(body))
        {
            await DisplayAlert("Error", "Feedback mustn't be empty!", "OK");
            return;
        }    
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

    public async Task FetchAPI()
    {
        string url = "https://asia-south1.gcp.data.mongodb-api.com/app/application-0-srjva/endpoint/GetVersion";

        try
        {
            using (HttpClient httpClient = new HttpClient())
            {

                string response = await httpClient.GetStringAsync(url);

                string currentVersion = AppInfo.VersionString;
                await Console.Out.WriteLineAsync(currentVersion);

                List<Version> version = JsonSerializer.Deserialize<List<Version>>(response);
                Console.WriteLine(version[0].version);

                if (version[0].version == currentVersion)
                {
                    await App.Current.MainPage.DisplayAlert("Update Information", "Your app is the latest version", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Update Information", "Your app is not the latest version. Check for update on our website", "OK");
                }
                httpClient.Dispose();
            }
        }
        catch (HttpRequestException)
        {
            await App.Current.MainPage.DisplayAlert("Update Information", "Network or server have problems", "OK");
            await App.Current.MainPage.DisplayAlert("Update Information", "Server have problems", "OK");
            return;
        }
    }

    private void onHomepageLabelTapped(object sender, TappedEventArgs e)
    {
        Launcher.OpenAsync(new Uri("https://lehuudouit.github.io/WebBDict/"));
    }

    private void updateBtn_Clicked(object sender, EventArgs e)
    {
        var currentNetwork = Connectivity.NetworkAccess;

        if (currentNetwork == NetworkAccess.Internet)
        {
            // Nếu có mạng, thực hiện yêu cầu API
            _ = FetchAPI();
        }
        else
        {
            // Hiển thị thông báo nếu không có mạng
            App.Current.MainPage.DisplayAlert("Network Error", "No internet connection available", "OK");
        }
    }

    private void onHomepageButton_Clicked(object sender, EventArgs e)
    {
        Launcher.OpenAsync(new Uri("https://lehuudouit.github.io/WebBDict/"));
    }

    private void switch_log_Toggled(object sender, ToggledEventArgs e)
    {
        if (switch_log.IsToggled == true)
        {
            DisplayAlert("Reminder", "Shake your phone to open LogPage", "OK");
        }
    }
}