namespace MyApp.MVVM.Views;

public partial class InfoView : ContentPage
{
	public InfoView()
	{
		InitializeComponent();
	}

    private void OnHomepageLabelTapped(object sender, EventArgs e)
    {
        Launcher.OpenAsync(new Uri("https://www.figma.com/file/nAs8ytuPgqFVShiz7mxf8j/Dictionary-(Community)?type=design&node-id=0-1&mode=design&t=bRYcYEfZVI8SLC5O-0"));
    }
}