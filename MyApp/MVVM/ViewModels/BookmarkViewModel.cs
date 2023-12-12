using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyApp.MVVM.Models;
using MyApp.MVVM.Views;

namespace MyApp.MVVM.ViewModels
{
    public partial class BookmarkViewModel : ObservableObject
    {
        [ObservableProperty]
        public string word;

        [ObservableProperty]
        public FavoriteWord selectedFavoriteWord;

        [ObservableProperty]
        public ObservableCollection<FavoriteWord> favoriteWords;

        [ObservableProperty]
        public bool isRefreshing;

        private readonly BookmarkDbServices _dbService;

        public BookmarkViewModel()
        {
            _dbService = new BookmarkDbServices();
            FavoriteWords = new ObservableCollection<FavoriteWord>();
            Task.Run(async () => await LoadFavoriteWords());
        }

        private async Task LoadFavoriteWords()
        {
            FavoriteWords = new ObservableCollection<FavoriteWord>(await _dbService.GetFavoriteWords());
        }

        [RelayCommand]
        void Add()
        {
            var tabbedPage = (TabbedPage)Application.Current.MainPage;

            var searchView = new SearchView();

            tabbedPage.CurrentPage = new NavigationPage(searchView);
        }

        [RelayCommand]
        void Search(FavoriteWord s)
        {
            if (FavoriteWords.Contains(s))
            {
                var resultView = new ResultView();
                resultView.BindingContext = new ResultViewModel(s.Word);
                App.Current.MainPage.Navigation.PushModalAsync(resultView);
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", "Unable to show result", "OK");
            }
        }

        [RelayCommand]
        async Task Delete(FavoriteWord s)
        {
            if (FavoriteWords.Contains(s))
            {
                await _dbService.Delete(s);
                await LoadFavoriteWords();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Unable to delete", "OK");
            }
        }

        [RelayCommand]
        void Refresh()
        {
            IsRefreshing = true;
            Task.Delay(3000);
            Task.Run(async () => await LoadFavoriteWords());
            IsRefreshing = false;
        }
    }
}
