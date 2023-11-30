using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyApp.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyApp.MVVM.ViewModels
{
    internal partial class ResultViewModel : ObservableObject
    {
        public string inputWord;
       
        [ObservableProperty]
        public string word;

        [ObservableProperty]
        public string phonetic;

        [ObservableProperty]
        public string audio;

        [ObservableProperty]
        public ObservableCollection<string> partOfSpeechs;

        [ObservableProperty]
        public ObservableCollection<string> definitions;

        [ObservableProperty]
        public ObservableCollection<string> examples;

        [ObservableProperty]
        public ObservableCollection<String> synonyms;

        [ObservableProperty]
        public ObservableCollection<String> antonyms;
        
       

        private static readonly HttpClient _httpClient = new HttpClient();
        public async Task FetchAPI()
        {
            string url = "https://api.dictionaryapi.dev/api/v2/entries/en/" + inputWord;
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                List<Word> word = JsonSerializer.Deserialize<List<Word>>(response);
                Console.WriteLine(response);
                
                //Các thuộc tính chỉ xuất hiện 1 lần

                Word = word[0].word;
                Phonetic = word[0].phonetics[0].text;
                Audio = word[0].phonetics[0].audio;

                //Các thuộc tính cần duyệt nhiều lần
                Synonyms.Add("Synonyms: ");
                Antonyms.Add("Antonyms: ");
                for (int i = 0; i < word[0].meanings.Count; i++)
                {
                    Definitions.Add(word[0].meanings[i].partOfSpeech.ToUpper() + ":");
                    for (int j = 0; j < word[0].meanings[i].definitions.Count; j++)
                    {
                        await Console.Out.WriteLineAsync(word[0].meanings[i].definitions[j].definition);
                        Definitions.Add("● " + word[0].meanings[i].definitions[j].definition);
                        if(word[0].meanings[i].definitions[j].example != null)
                            Definitions.Add("➜ " + word[0].meanings[i].definitions[j].example);
                        else
                        {
                            Definitions.Add("?");
                        }
                    }
                    for (int j = 0; j < word[0].meanings[i].synonyms.Length; j++)
                        Synonyms.Add(word[0].meanings[i].synonyms[j] + ", ");
                    for (int j = 0; j < word[0].meanings[i].antonyms.Length; j++)
                        Antonyms.Add(word[0].meanings[i].antonyms[j] + ", ");
                }
                if (Synonyms.Count == 0)
                    Synonyms.Add("?");
                else
                    Synonyms[Synonyms.Count - 1] = Synonyms[Synonyms.Count - 1].TrimEnd(' ').TrimEnd(',') + ".";

                if (Antonyms.Count ==0) 
                    Antonyms.Add ("?");
                else
                    Antonyms[Antonyms.Count - 1] = Antonyms[Antonyms.Count - 1].TrimEnd(' ').TrimEnd(',') + ".";
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine("Error {0}", er);
            }
        }

        public ResultViewModel()
        {
            inputWord = "hello";
            PartOfSpeechs = new ObservableCollection<string>();
            Definitions = new ObservableCollection<string>();
            Examples = new ObservableCollection<string>();
            Synonyms = new ObservableCollection<string>();
            Antonyms = new ObservableCollection<string>();
            _ = FetchAPI();
        }
        public ResultViewModel(string input)
        {
            inputWord = input;
            PartOfSpeechs = new ObservableCollection<string>();
            Definitions = new ObservableCollection<string>();
            Examples = new ObservableCollection<string>();
            Synonyms = new ObservableCollection<string>();
            Antonyms = new ObservableCollection<string>();
            _ = FetchAPI();
        }
    }
}
