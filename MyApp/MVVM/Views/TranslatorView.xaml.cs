using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class TranslatorView : ContentPage
{
    public TranslatorView()
    {
        InitializeComponent();
        BindingContext = new TranslatorViewModel();
    }
}