using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class ResultView : ContentPage
{
	public ResultView()
	{
		InitializeComponent();
		BindingContext = new ResultViewModel();
	}

    private void backIcon_Clicked(object sender, EventArgs e)
    {
		Navigation.PopModalAsync();
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Kiểm tra xem có mục nào được chọn không
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            // Lấy mục được chọn
            var selectedWord = (string)e.CurrentSelection.FirstOrDefault();

            // Tạo view model và trang chi tiết
            var resultViewModel = new ResultViewModel(selectedWord);
            var resultView = new ResultView();
            resultView.BindingContext = resultViewModel;

            // Chuyển đến trang chi tiết
            Navigation.PushModalAsync(resultView);

            // Đặt lại chọn để tránh xử lý lặp
            ((CollectionView)sender).SelectedItem = null;
        }   
    }

    private void playSound_Clicked(object sender, EventArgs e)
    {
        mediaElement.Play();
        mediaElement.SeekTo(TimeSpan.Zero);
    }
}