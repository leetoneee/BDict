using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class SearchView : ContentPage
{
	public SearchView()
	{
		InitializeComponent();
		BindingContext = new SearchViewModel();
	}

    private void searchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        searchBar.Unfocus();
        var resultView = new ResultView();
        resultView.BindingContext = new ResultViewModel(searchBar.Text);
        Navigation.PushModalAsync(resultView);
    }
}