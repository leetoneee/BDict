using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class BookmarkView : ContentPage
{
	public BookmarkView()
	{
		InitializeComponent();
		BindingContext = new BookmarkViewModel();
	}
}