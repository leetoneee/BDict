namespace MyApp.MVVM.Views;

public partial class TranslatorView : ContentPage
{
	public TranslatorView()
	{
		InitializeComponent();
	}

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
		DisplayAlert("Alert", "copy click", "ok");
    }
}