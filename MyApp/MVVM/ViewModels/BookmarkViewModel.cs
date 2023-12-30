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
        public int rowCount;

        [ObservableProperty]
        public FavoriteWord selectedFavoriteWord;

        private FavoriteWord _draggedItem;

        [ObservableProperty]
        public ObservableCollection<FavoriteWord> favoriteWords;

        [ObservableProperty]
        public ObservableCollection<char> _alphabet;

        [ObservableProperty]
        private char selectedSortByWord;

        [ObservableProperty]
        public bool isDragged;

        [ObservableProperty]
        public bool isRefreshing;

        public bool IsSortA2Z, IsSortZ2A;

        [ObservableProperty]
        public string sourceSorting;
        
        private readonly BookmarkDbServices _dbService;

        public BookmarkViewModel()
        {
            _dbService = new BookmarkDbServices();
            FavoriteWords = new ObservableCollection<FavoriteWord>();
            SourceSorting = "sorta2z.svg";
            IsDragged = false;
            IsSortA2Z = false;
            IsSortZ2A = false;
            Task.Run(async () => await LoadFavoriteWords());
            Task.Run(async () => await GetTheNumberOfWord());
            InitializeAlphabet();
            SelectedSortByWord = Alphabet.FirstOrDefault();
        }

        [RelayCommand]
        public void DragStarted(FavoriteWord FW)
        {
            _draggedItem = FW;
            IsDragged = true;
        }

        [RelayCommand]
        public void DragCompleted()
        {
            _draggedItem = null;
            IsDragged = false;
        }
        private void InitializeAlphabet()
        {
            var alphabetList = Enumerable.Range('A', 26).Select(c => (char)c).ToList();
            alphabetList.Insert(0, '#'); 
            Alphabet = new ObservableCollection<char>(alphabetList);
        }
        private async Task GetTheNumberOfWord()
        {
            RowCount = await _dbService.GetRowCountAsync();
        }
        private async Task LoadFavoriteWords()
        {
            FavoriteWords = new ObservableCollection<FavoriteWord>(await _dbService.GetFavoriteWords());
            await GetTheNumberOfWord();
        }
        private async Task LoadFavoriteWordsA2Z()
        {
            FavoriteWords = new ObservableCollection<FavoriteWord>(await _dbService.GetFavoriteWordsA2Z());
            await GetTheNumberOfWord();
        }
        private async Task LoadFavoriteWordsZ2A()
        {
            FavoriteWords = new ObservableCollection<FavoriteWord>(await _dbService.GetFavoriteWordsZ2A());
            await GetTheNumberOfWord();
        }

        private async Task LoadFavoriteWordsSortedByWord(string s)
        {
            FavoriteWords = new ObservableCollection<FavoriteWord>(await _dbService.GetWordsStartingWithAsync(s));
            await GetTheNumberOfWord();
        }

        [RelayCommand]
        void Add()
        {
            var tabbedPage = (TabbedPage)Application.Current.MainPage;

            var searchView = new SearchView();

            tabbedPage.CurrentPage = new NavigationPage(searchView);
        }

        [RelayCommand]
        async Task SortByWord()
        {
            IsRefreshing = true;
            char c = SelectedSortByWord;
            if (Alphabet.Contains(c))
            {
                if (c == '#')
                {
                    await LoadFavoriteWords();
                    return;
                }
                string startingChar = c.ToString();
                await LoadFavoriteWordsSortedByWord(startingChar);
            }
            IsRefreshing = false;
        }

        [RelayCommand]
        void Search(FavoriteWord s)
        {
            Console.WriteLine(s.Word);
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
                SelectedSortByWord = Alphabet.FirstOrDefault();
                await LoadFavoriteWords();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Unable to delete", "OK");
            }
        }

        [RelayCommand]
        public async Task TaskDropped()
        {
            IsDragged = false;
            if (FavoriteWords.Contains(_draggedItem))
            {
                await _dbService.Delete(_draggedItem);
                await SortByWord();
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
            IsSortA2Z = false;
            IsSortZ2A = false;
            SourceSorting = "sorta2z.svg";
            Task.Delay(3000);
            Task.Run(async () => await LoadFavoriteWords());
            IsRefreshing = false;
        }

        [RelayCommand]
        async Task Sorting()
        {
            if ((!IsSortA2Z && !IsSortZ2A) || IsSortZ2A)
            {
                IsRefreshing = true;
                IsSortA2Z = true;
                IsSortZ2A = false;
                SourceSorting = "sortz2a.svg";
                await LoadFavoriteWordsA2Z();
                await Task.Delay(1000);
                IsRefreshing = false;
            }
            else
            {
                IsRefreshing = true;
                IsSortZ2A = true;
                IsSortA2Z = false;
                SourceSorting = "sorta2z.svg";
                await LoadFavoriteWordsZ2A();
                await Task.Delay(1000);
                IsRefreshing = false;
            }
        }
    }
}
