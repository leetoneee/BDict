using CommunityToolkit.Mvvm.ComponentModel;
using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class ResultView : ContentPage
{
	public ResultView()
	{
		InitializeComponent();
		BindingContext = new ResultViewModel();
	}

    private async void backIcon_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }
}