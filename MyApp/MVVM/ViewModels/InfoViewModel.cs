using System.Text.Json;
using System.Windows.Input;
using Version = MyApp.MVVM.Models.Version;

namespace MyApp.MVVM.ViewModels
{
    public partial class InfoViewModel
    {
        public ICommand HomepageCommand { get; }
        public ICommand UpdateCommand { get; }

        public InfoViewModel() 
        {
            HomepageCommand = new Command(onHomepageLabelTapped);
            UpdateCommand = new Command(updateBtn_Clicked);
        }
        private void onHomepageLabelTapped()
        {
            Launcher.OpenAsync(new Uri("https://lehuudouit.github.io/WebBDict/"));
        }

        private void updateBtn_Clicked()
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
    }
}
