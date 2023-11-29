using MyApp.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.MVVM.ViewModels
{
    public class SearchViewModel
    {
        public ObservableCollection<Word> RecentSearches { get; set; }

        public SearchViewModel() {
            RecentSearches = new ObservableCollection<Word>()
            {
                new Word{
                    word = "Hello"
                },
                new Word{
                    word = "Machine"
                },
                new Word{
                    word = "Good"
                },
                new Word{
                    word = "Build"
                },
            };
        }
    }
}
