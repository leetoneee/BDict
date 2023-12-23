﻿using System.Net.Http;
using System.Net.Http.Headers;

namespace MyApp.MVVM.Views;

public partial class InfoView : ContentPage
{
    public InfoView()
	{
		InitializeComponent();
	}

    private void OnHomepageLabelTapped(object sender, EventArgs e)
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
            DisplayAlert("Network Error", "No internet connection available", "OK");
        }
    }

    public async Task FetchAPI()
    {
        string url = "https://gist.githubusercontent.com/lehuudoUIT/2f01b66c39aba9dc61d158a302f6be4f/raw";

        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true };
                
                string response = await httpClient.GetStringAsync(url);
                await Console.Out.WriteLineAsync(response);

                string currentVersion = AppInfo.VersionString;
                await Console.Out.WriteLineAsync(currentVersion);

                if (response == currentVersion)
                {
                    await DisplayAlert("Update Information", "Your app is the latest version", "OK");
                }
                else
                {
                    await DisplayAlert("Update Information", "Your app is not the latest version. Check for update on our website", "OK");
                }
                httpClient.Dispose();
            }
        }
        catch (HttpRequestException)
        {
            await DisplayAlert("Update Information", "Network or server have problems", "OK");
            return;
        }
    }
}