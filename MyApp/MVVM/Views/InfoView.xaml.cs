using MyApp.MVVM.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyApp.MVVM.Views;

public partial class InfoView : ContentPage
{
    public InfoView()
	{
		InitializeComponent();
		BindingContext = new InfoViewModel();
	}
}