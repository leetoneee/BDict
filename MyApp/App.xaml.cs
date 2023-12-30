using MyApp.MVVM.Views;

namespace MyApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (DeviceInfo.Platform == DevicePlatform.Android)
                MainPage = new LoadingView();
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
                MainPage = new FlyoutView();
        }
    }
}