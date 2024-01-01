using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using MyApp.MVVM.Models;
using MyApp.MVVM.Views;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;


namespace MyApp.MVVM.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        [ObservableProperty]
        private RecentWord inputWord;

        [ObservableProperty]
        private RecentWord selectedWord;

        [ObservableProperty]
        private ObservableCollection<RecentWord> recentWords;

        [ObservableProperty]
        private string randomWord;

        [ObservableProperty]
        public ObservableCollection<string> randomWords;

        private readonly RecentDbServices _recentWordService;
        private int _editWordId;

        [ObservableProperty]
        public bool isFavorite;

        [ObservableProperty]
        public string sourceFavorite;

        [ObservableProperty]
        public FavoriteWord favoriteWord;

        private readonly BookmarkDbServices _bookmarkService;

        public ICommand SearchCommand { get; }
        public ICommand SelectionChangedCommand { get; }
        public ICommand GetRandomWordCommand { get; }
        public ICommand FavoriteCommand { get; }
        public ICommand SearchRandomWordCommand { get; }

        public SearchViewModel()
        {
            _bookmarkService = new BookmarkDbServices();
            _recentWordService = new RecentDbServices();
            InputWord = new RecentWord();
            RecentWords = new ObservableCollection<RecentWord>();
            RandomWords = new ObservableCollection<string>();

            Task.Run(async () => await LoadRecentWords());
            Task.Run(async () => await LoadRandomWords());
            RandomWord = "Hello";

            _ = CheckIfFavoriteAsync();

            SearchCommand = new Command(search_Clicked);
            SelectionChangedCommand = new Command(selectionChanged);
            GetRandomWordCommand = new Command(getRandomWord);
            FavoriteCommand = new Command(favoriteCommand);
            SearchRandomWordCommand = new Command(searchRandomWord);
        }

        private async Task CheckIfFavoriteAsync()
        {
            FavoriteWord = await _bookmarkService.GetByWord(RandomWord);
            IsFavorite = FavoriteWord != null;
            SourceFavorite = IsFavorite ? "heart_fill.png" : "heart_unfill.png";
        }

        public async void favoriteCommand()
        {
            if (IsFavorite)
            {
                SourceFavorite = "heart_unfill.png";
                IsFavorite = false;
                await _bookmarkService.DeleteByWordAsync(RandomWord);
            }
            else
            {
                SourceFavorite = "heart_fill.png";
                IsFavorite = true;
                await _bookmarkService.Create(new FavoriteWord
                {
                    Word = RandomWord
                });
            }
        }

        private async Task LoadRecentWords()
        {
            RecentWords = new ObservableCollection<RecentWord>(await _recentWordService.GetRecentWordsAsync());
        }

        private async Task LoadRandomWords()
        {
            string line;
            using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("10000words.txt");
            using StreamReader reader = new StreamReader(fileStream);
            while ((line = reader.ReadLine()) != null)
            {
                RandomWords.Add(line);
            }
        }

        private async Task NavigateToResultView(string inputWord)
        {
            var resultView = new ResultView();
            resultView.BindingContext = new ResultViewModel(inputWord);
            await App.Current.MainPage.Navigation.PushModalAsync(resultView);
        }

        private async void search_Clicked()
        {
            if (string.IsNullOrEmpty(InputWord.Word))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please input a word", "OK");
                return;
            }

            string normalizedWord = NormalizeWord(InputWord.Word);

            if (_editWordId == 0)
            {
                await _recentWordService.Create(new RecentWord
                {
                    Word = normalizedWord
                });
            }
            else
            {
                await _recentWordService.Update(new RecentWord
                {
                    Word = normalizedWord
                });
                _editWordId = 0;
            }
            
            await NavigateToResultView(normalizedWord);
            InputWord = new RecentWord();
            await LoadRecentWords();
        }

        private async void searchRandomWord()
        {
            if (string.IsNullOrEmpty(RandomWord))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please input a word", "OK");
                return;
            }

            if (_editWordId == 0)
            {
                await _recentWordService.Create(new RecentWord
                {
                    Word = RandomWord
                });
            }
            else
            {
                await _recentWordService.Update(new RecentWord
                {
                    Word = RandomWord
                });
                _editWordId = 0;
            }

            await NavigateToResultView(RandomWord);

            await LoadRecentWords();
        }

        private void selectionChanged()
        {
            RecentWord word = SelectedWord;
            if (word != null)
            {
                InputWord = word;
                _editWordId = 0;
                SelectedWord = null;
            }
        }

        private void getRandomWord()
        {
            string newWord = GetRandomWord(RandomWords);
            newWord = NormalizeWord(newWord);
            if (newWord != null)
            {
                RandomWord = newWord;
            }
            _ = CheckIfFavoriteAsync();
        }

        static string NormalizeWord(string inputWord)
        {
            string trimmedWord = inputWord.Trim();

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string normalizedWord = textInfo.ToTitleCase(trimmedWord.ToLower());

            return normalizedWord;
        }

        static string GetRandomWord(ObservableCollection<string> collection)
        {
            if (collection.Count == 0)
            {
                return null;
            }

            Random random = new Random();
            int index = random.Next(collection.Count);
            return collection[index];
        }
    }
}
