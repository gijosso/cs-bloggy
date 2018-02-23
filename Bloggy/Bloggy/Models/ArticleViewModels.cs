using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bloggy.Models
{
    public class UserArticleViewModel
    {
        public Models.Article Article { get; set; }
        public ApplicationUser Author { get; set; }
        public IEnumerable<Element> Elements { get; set; }
    }
}