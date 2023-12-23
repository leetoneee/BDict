namespace MyApp.MVVM.Views;

public partial class InfoView : ContentPage
{
	public InfoView()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void OnHomepageLabelTapped(object sender, EventArgs e)
    {
        Launcher.OpenAsync(new Uri("https://lehuudouit.github.io/WebBDict/"));
    }
}