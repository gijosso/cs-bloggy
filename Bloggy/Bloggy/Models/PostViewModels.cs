using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bloggy.Models
{
    public class PostsViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
    }

    public class PostViewModel
    {
        public Post Post { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }

    public class ArticleViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<Element> Elements { get; set; }
    }

    public class ElementViewModel
    {
        public HttpPostedFileBase file { get; set; }
        public Element element { get; set; }
    }

    public class AddArticleViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<LangList> LangList { get; set; }
    }
}
