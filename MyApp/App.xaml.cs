using MyApp.MVVM.Views;

namespace MyApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TabbedView();
        }
    }
}