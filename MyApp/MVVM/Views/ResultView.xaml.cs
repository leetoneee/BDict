using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class ResultView : ContentPage
{
	public ResultView()
	{
		InitializeComponent();
		BindingContext = new ResultViewModel();
	}

    private void playSound_Clicked(object sender, EventArgs e)
    {
        mediaElement.Play();
        mediaElement.SeekTo(TimeSpan.Zero);
    }
}