using MyApp.MVVM.Models;
using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class BookmarkView : ContentPage
{
    public BookmarkView()
    {
        InitializeComponent();
        BindingContext = new BookmarkViewModel();
        //this.Appearing += OnBookmarkViewAppearing;
    }

    //private void OnBookmarkViewAppearing(object sender, EventArgs e)
    //{
    //    BindingContext = new BookmarkViewModel();
    //}
}