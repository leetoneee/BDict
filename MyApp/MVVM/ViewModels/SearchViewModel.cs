using CommunityToolkit.Mvvm.ComponentModel;
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

        private readonly RecentDbServices _recentWordService;
        private int _editWordId;

        public ICommand SearchCommand { get; }
        public ICommand SelectionChangedCommand { get; }

        public SearchViewModel()
        {
            _recentWordService = new RecentDbServices();
            InputWord = new RecentWord();
            RecentWords = new ObservableCollection<RecentWord>();
            Task.Run(async () => await LoadRecentWords());
            SearchCommand = new Command(search_Clicked);
            SelectionChangedCommand = new Command(selectionChanged);
        }

        private async Task LoadRecentWords()
        {
            RecentWords = new ObservableCollection<RecentWord>(await _recentWordService.GetRecentWordsAsync());
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

        static string NormalizeWord(string inputWord)
        {
            // Loại bỏ khoảng trắng ở đầu và cuối
            string trimmedWord = inputWord.Trim();

            // Đảm bảo chữ cái đầu viết hoa, các chữ cái sau viết thường
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string normalizedWord = textInfo.ToTitleCase(trimmedWord.ToLower());

            return normalizedWord;
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
    }
}
