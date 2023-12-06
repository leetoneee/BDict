using CommunityToolkit.Mvvm.ComponentModel;
using MyApp.MVVM.Models;
using MyApp.MVVM.Views;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace MyApp.MVVM.ViewModels
{
    internal partial class ResultViewModel : ObservableObject
    {
        public string inputWord;

        [ObservableProperty]
        public string word;

        [ObservableProperty]
        public string selectedWord;

        [ObservableProperty]
        public string phonetic;

        [ObservableProperty]
        public string audio;

        [ObservableProperty]
        public ObservableCollection<string> definitions;

        [ObservableProperty]
        public ObservableCollection<String> synonyms;

        [ObservableProperty]
        public ObservableCollection<String> antonyms;

        [ObservableProperty]
        public bool isProcessing;

        [ObservableProperty]
        public bool isVisibleElement;

        [ObservableProperty]
        public bool isFavorite;

        [ObservableProperty]
        public string sourceFavorite;

        [ObservableProperty]
        public FavoriteWord favoriteWord;

        private readonly BookmarkDbServices _dbService;
        private int _editWordId;

        private static readonly HttpClient _httpClient = new HttpClient();
        public ICommand FavoriteCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SelectionChangedCommand { get; }
        public async Task FetchAPI()
        {
            string url = "https://api.dictionaryapi.dev/api/v2/entries/en/" + inputWord;
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                List<Word> word = JsonSerializer.Deserialize<List<Word>>(response);
                Console.WriteLine(response);

                //Các thuộc tính chỉ xuất hiện 1 lần

                Word = word[0].word.ToUpper();

                int flag = 0;
                for (int i = 0; i < word[0].phonetics.Count; i++)
                {
                    if (!(word[0].phonetics[i].audio != "" && word[0].phonetics[i].text != ""
                    && word[0].phonetics[i].audio != null && word[0].phonetics[i].text != null))
                    {
                        Audio = word[0].phonetics[i].audio;
                        Phonetic = word[0].phonetics[i].text;
                        flag = 1;
                        break;
                    }
                }

                if (flag != 1 && word[0].phonetics.Count != 0)
                {
                    Audio = word[0].phonetics[0].audio;
                    Phonetic = word[0].phonetics[0].text;
                }


                HashSet<String> synonymsHash = new HashSet<String>();
                HashSet<String> antonymsHash = new HashSet<String>();

                //Các thuộc tính cần duyệt nhiều lần
                for (int i = 0; i < word[0].meanings.Count; i++)
                {
                    Definitions.Add(word[0].meanings[i].partOfSpeech.ToUpper() + ":");
                    for (int j = 0; j < word[0].meanings[i].definitions.Count; j++)
                    {
                        Definitions.Add("● " + word[0].meanings[i].definitions[j].definition);
                        if (word[0].meanings[i].definitions[j].example != null)
                            Definitions.Add("➜ " + word[0].meanings[i].definitions[j].example);
                    }
                    for (int j = 0; j < word[0].meanings[i].synonyms.Length; j++)
                    {
                        Console.WriteLine(word[0].meanings[i].synonyms[j]);
                        synonymsHash.Add(word[0].meanings[i].synonyms[j]);
                    }
                    for (int j = 0; j < word[0].meanings[i].antonyms.Length; j++)
                        antonymsHash.Add(word[0].meanings[i].antonyms[j]);
                }

                foreach (string x in synonymsHash)
                    Synonyms.Add(x);

                foreach (string x in antonymsHash)
                    Antonyms.Add(x);


                if (Synonyms.Count == 0)
                    Synonyms.Add("?");

                if (Antonyms.Count == 0)
                    Antonyms.Add("?");

                IsProcessing = false;
                IsVisibleElement = true;
                return;
            }
            catch (HttpRequestException)
            {
                IsProcessing = false;
                Word = "404 NOT FOUND";
                return;
            }
        }

        //Constructor mặc định
        public ResultViewModel()
        {
            IsFavorite = false;
            FavoriteCommand = new Command(favoriteCommand);
        }
        // Constructor nhận 1 tham số đầu vào
        public ResultViewModel(string input)
        {
            _dbService = new BookmarkDbServices();
            inputWord = input;
            Definitions = new ObservableCollection<string>();
            Synonyms = new ObservableCollection<string>();
            Antonyms = new ObservableCollection<string>();

            // Fetch API data
            IsProcessing = true;
            IsVisibleElement = false;
            _ = FetchAPI();

            // Check if the word is a favorite
            _ = CheckIfFavoriteAsync();

            // Set up commands
            FavoriteCommand = new Command(favoriteCommand);
            BackCommand = new Command(backIcon_Clicked);
            SelectionChangedCommand = new Command(CollectionView_SelectionChanged);
        }
        private async Task CheckIfFavoriteAsync()
        {
            FavoriteWord = await _dbService.GetByWord(inputWord); 
            IsFavorite = FavoriteWord != null;
            SourceFavorite = IsFavorite ? "heart_fill.svg" : "heart_unfill.svg";
        }

        public async void favoriteCommand()
        {
            if (IsFavorite)
            {
                SourceFavorite = "heart_unfill.svg";
                IsFavorite = false;
                await _dbService.DeleteByWordAsync(inputWord);
            }
            else
            {
                SourceFavorite = "heart_fill.svg";
                IsFavorite = true;
                await _dbService.Create(new FavoriteWord
                {
                    Word = inputWord
                });
            }
        }

        private void backIcon_Clicked()
        {
            App.Current.MainPage.Navigation.PopModalAsync();
        }

        private void CollectionView_SelectionChanged()
        {
            string temp = SelectedWord;
            if (temp != null)
            {
                // Tạo view model và trang chi tiết
                var resultViewModel = new ResultViewModel(temp);
                var resultView = new ResultView();
                resultView.BindingContext = resultViewModel;

                // Chuyển đến trang chi tiết
                App.Current.MainPage.Navigation.PushModalAsync(resultView);

                // Đặt lại chọn để tránh xử lý lặp
                SelectedWord = null;
            }
        }
    }
}
