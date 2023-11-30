using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class ResultView : ContentPage
{
	public ResultView()
	{
		InitializeComponent();
		BindingContext = new ResultViewModel();
	}

    private void backIcon_Clicked(object sender, EventArgs e)
    {
		Navigation.PopModalAsync();
    }
}