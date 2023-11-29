using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.MVVM.Models
{
    public class Word
    {
        public string word { get; set; }
        public List<Phonetics> phonetics { get; set; }
        public List<Meanings> meanings { get; set; }
    }

    public class Phonetics
    {
        public string text { get; set; }
        public string audio { get; set; }
    }

    public class Meanings
    {
        public string partOfSpeech { get; set; }
        public List<Definitions> definitions { get; set; }
        public List<string> synonyms { get; set; }
        public List<string> antonyms { get; set; }
    }

    public class Definitions
    {
        public string definition { get; set; }
        public string example { get; set; }
    }
    
}
