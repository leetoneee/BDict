using CommunityToolkit.Mvvm.ComponentModel;
using MyApp.MVVM.Models;
using System.Collections.ObjectModel;
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

        private async void search_Clicked()
        {
            if (string.IsNullOrEmpty(InputWord.Word))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please input a word", "OK");
                return;
            }
            if (_editWordId == 0)
            {
                await _recentWordService.Create(new RecentWord
                {
                    Word = InputWord.Word
                });
            }
            else
            {
                await _recentWordService.Update(new RecentWord
                {
                    Word = InputWord.Word
                });
                _editWordId = 0;
            }
            InputWord = new RecentWord();
            await LoadRecentWords();
        }
        private void selectionChanged()
        {
            RecentWord word = SelectedWord;
            if (word != null)
            {
                InputWord = word;
                _editWordId = word.Id;
                SelectedWord = null;
            }
        }
    }
}
