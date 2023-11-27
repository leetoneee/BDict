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

    private void twoArrowBtn_Clicked(object sender, EventArgs e)
    {
        string temp1 = upLabel.Text;
        upLabel.Text = downLabel.Text;
        downLabel.Text = temp1;

        string temp2 = firstBtn.Text;
        firstBtn.Text = secondBtn.Text;
        secondBtn.Text = temp2;
    }
}