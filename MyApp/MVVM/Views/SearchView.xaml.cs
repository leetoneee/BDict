using MyApp.MVVM.Models;
using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class SearchView : ContentPage
{
	public SearchView()
	{
		InitializeComponent();
		BindingContext = new SearchViewModel();
	}


    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(e.CurrentSelection != null && e.CurrentSelection.Count > 0)
    {
            // Xử lý khi một mục được chọn
            var selectedWord = (Word)e.CurrentSelection.First();
            // Làm bất cứ điều gì bạn muốn với từ đã chọn
            // Ví dụ: Hiển thị thông tin chi tiết, chuyển đến trang chi tiết, vv.
            await DisplayAlert("Thông báo", $"Bạn đã chọn từ: {selectedWord.word}", "OK");
            // Clear selection to enable selecting the same item again
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Thông báo", "Bạn đã chạm vào ImageButton", "OK");
    }

    private async void searchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ResultView());
        searchBar.Unfocus();
    }
}