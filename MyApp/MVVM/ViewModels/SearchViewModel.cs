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
        public ObservableCollection<Vocabulary> RecentSearches { get; set; }

        public SearchViewModel() {
            RecentSearches = new ObservableCollection<Vocabulary>()
            {
                new Vocabulary{
                    Word = "Hello"
                },
                new Vocabulary{
                    Word = "Machine"
                },
                new Vocabulary{
                    Word = "Good"
                },
                new Vocabulary{
                    Word = "Build"
                },
            };
        }
    }
}
