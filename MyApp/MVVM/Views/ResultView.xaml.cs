namespace MyApp.MVVM.Views;

public partial class ResultView : ContentPage
{
	public ResultView()
	{
		InitializeComponent();
	}

    private async void backIcon_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }
}