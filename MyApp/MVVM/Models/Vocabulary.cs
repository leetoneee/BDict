using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.MVVM.Models
{
    public class Vocabulary
    {
        public string Word { get; set; }
        public string Phonetic { get; set; }
        public string Audio { get; set; }
        public string PartOfSpeech1 { get; set; }
        public string Definition1 { get; set; }
        public string Example1 { get; set; }
        public string PartOfSpeech2 { get; set; }
        public string Definition2 { get; set; }
        public string Example2 { get; set; }
        public string PartOfSpeech3 { get; set; }
        public string Definition3 { get; set; }
        public string Example3 { get; set; }
        public string Synonyms { get; set;}
        public string Antonyms { get; set; }
    }
}
