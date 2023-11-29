using System.Windows.Input;
using MyApp.MVVM.Views;

namespace MyApp.MVVM.ViewModels
{
    public class BookmarkViewModel
    {
        public ICommand AddCommand { get; }

        public BookmarkViewModel()
        {
            AddCommand = new Command(AddBtn_Clicked);
        }

        public async void AddBtn_Clicked()
        {
            // Lấy tham chiếu đến trang hiện tại của TabbedPage
            var tabbedPage = (TabbedPage)Application.Current.MainPage;

            // Tạo trang mới
            var searchView = new SearchView();

            // Thiết lập trang mới làm trang hiện tại
            tabbedPage.CurrentPage = new NavigationPage(searchView);
        }
    }
}
