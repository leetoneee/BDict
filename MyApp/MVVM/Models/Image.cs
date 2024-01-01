using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.MVVM.Models
{
    public class Image
    {
        public List<Photos> photos { get; set; }
    }

    public class Photos
    {
        public Src src{ get; set; }
    }

    public class Src
    {
        public string original { get; set; }
    }
}
