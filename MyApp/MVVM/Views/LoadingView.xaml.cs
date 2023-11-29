namespace MyApp.MVVM.Views;

public partial class LoadingView : ContentPage
{
	public LoadingView()
	{
		InitializeComponent();
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await Task.Delay(3000);

        Application.Current.MainPage = new TabbedView();
    }
}