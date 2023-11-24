namespace MyApp.MVVM.Views;

public partial class TabbedView : TabbedPage
{
	public TabbedView()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Tab2.Navigation.PushAsync(new MainPage());
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Tab2.Navigation.PushAsync(new MainPage());
    }
}