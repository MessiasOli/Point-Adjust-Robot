using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Nexti
{
    internal class ResponseNexti <T>
    {
        public string numberOfElements {get; set;}
        public string totalElements {get; set;}
        public string totalPages {get; set;}
        public string last {get; set;}
        public string sort {get; set;}
        public string first {get; set;}
        public string size {get; set;}
        public string number { get; set; }
        public List<T> content {get; set;} = new List<T>(); 
    }
}
