using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bloggy.Models
{
    public class SearchViewModels
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public bool BeforePublishDate { get; set; } = true;
        public DateTime PublishDate { get; set; }
        public bool HasDocuments { get; set; }
        public bool HasImages { get; set; }
        public string KeyWords { get; set; }

        public IEnumerable<Article> Articles { get; set; }
    }
}
