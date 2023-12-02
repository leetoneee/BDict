﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        public ObservableCollection<string> definitions;

        [ObservableProperty]
        public ObservableCollection<String> synonyms;

        [ObservableProperty]
        public ObservableCollection<String> antonyms;
        
        [ObservableProperty]
        public bool isProcessing;
        
        [ObservableProperty]
        public bool isVisibleElement;



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

                Word = word[0].word.ToUpper();
                
                int count = 0;
                if(word[0].phonetics.Count != 0)
                {
                    while (!(word[0].phonetics[count].audio != "" && word[0].phonetics[count].text != ""
                    && word[0].phonetics[count].audio != null && word[0].phonetics[count].text != null))
                    {
                        count++;
                        if (count == 5)
                        {
                            count = 0;
                            break;
                        }
                    }
                    Audio = word[0].phonetics[count].audio;
                    Phonetic = word[0].phonetics[count].text;
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
                        synonymsHash.Add(word[0].meanings[i].antonyms[j]);
                }

                foreach (string x in synonymsHash)
                    Synonyms.Add(x);

                foreach (string x in antonymsHash)
                    Antonyms.Add(x);


                if (Synonyms.Count == 0)
                    Synonyms.Add("?");

                if (Antonyms.Count == 0) 
                    Antonyms.Add ("?");

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
        }
        //Constructor nhận 1 tham số đầu vào
        public ResultViewModel(string input)
        {
            inputWord = input;
            Definitions = new ObservableCollection<string>();
            Synonyms = new ObservableCollection<string>();
            Antonyms = new ObservableCollection<string>();
            IsProcessing = true;
            IsVisibleElement = false;
            _ = FetchAPI();

        }
    }
}
