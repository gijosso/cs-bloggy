using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bloggy.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<LangList> LangList { get; set; }
    }
}