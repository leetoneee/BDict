using MetroLog.Maui;
using MyApp.MVVM.Views;

namespace MyApp
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzAwNzQ1OEAzMjM0MmUzMDJlMzBuYjQ5U2ZtWmx4V2VWdUszck04L0lyemNOa3JzbEhaTW5iQzhzK01KZXZVPQ==");
            InitializeComponent();

            if (DeviceInfo.Platform == DevicePlatform.Android)
                MainPage = new LoadingView();
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
                MainPage = new FlyoutView();

            LogController.InitializeNavigation(
                page => MainPage!.Navigation.PushModalAsync(page),
                () =>  MainPage!.Navigation.PopModalAsync());
        }
    }
}