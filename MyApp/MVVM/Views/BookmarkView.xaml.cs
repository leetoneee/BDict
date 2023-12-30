using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls.Shapes;
using MyApp.MVVM.Models;
using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class BookmarkView : ContentPage
{
    public BookmarkView()
    {
        InitializeComponent();
        BindingContext = new BookmarkViewModel();
        NavigationPage.SetHasNavigationBar(this, false);
    }
}