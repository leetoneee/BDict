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
        public string[] synonyms;

        [ObservableProperty]
        public string[] antonyms;

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

                for(int i = 0; i < word[0].meanings.Count; i++)
                {
                    Definitions.Add(word[0].meanings[i].partOfSpeech.ToUpper() + ":");
                    for (int j = 0; j < word[0].meanings[i].definitions.Count; j++)
                    {
                        await Console.Out.WriteLineAsync(word[0].meanings[i].definitions[j].definition);
                        Definitions.Add(word[0].meanings[i].definitions[j].definition);
                        if(word[0].meanings[i].definitions[j].example != null)
                            Definitions.Add("Ex: "+ word[0].meanings[i].definitions[j].example);
                        else
                        {
                            Definitions.Add("?");
                        }    
                    }

                }
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine("Error {0}", er);
            }
        }

        public ResultViewModel()
        {
            inputWord = "fuck";
            PartOfSpeechs = new ObservableCollection<string>();
            Definitions = new ObservableCollection<string>();
            Examples = new ObservableCollection<string>();
            _ = FetchAPI();
        }
    }
}
