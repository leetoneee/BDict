using MyApp.MVVM.Models;
using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class BookmarkView : ContentPage
{
    public BookmarkView()
    {
        InitializeComponent();
        BindingContext = new BookmarkViewModel();
        this.Appearing += OnBookmarkViewAppearing;
    }

    private void OnBookmarkViewAppearing(object sender, EventArgs e)
    {
        InitializeComponent();
        BindingContext = new BookmarkViewModel();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Frame tappedFrame)
        {
            if (tappedFrame.BindingContext is FavoriteWord selectedItem)
            {
                string selectedWord = selectedItem.Word;
                var resultView = new ResultView();
                resultView.BindingContext = new ResultViewModel(selectedWord);
                Navigation.PushModalAsync(resultView);
            }

        }
    }
}