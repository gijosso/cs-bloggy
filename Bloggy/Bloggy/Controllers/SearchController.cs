using Bloggy.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bloggy.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;

        public ApplicationDbContext Context
        {
            get
            {
                return _context ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _context = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Search
        public ActionResult Index()
        {
            return View(new SearchViewModels()
            {
                PublishDate = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SearchViewModels model)
        {
            try
            {

                var list = Context.Articles.ToList();

                if (!(User.IsInRole("Admin") || User.IsInRole("Translator")))
                    list = list.Where(a => a.validated).ToList();

                    if (!string.IsNullOrEmpty(model.Title))
                    list = list.Where(a => a.name.Contains(model.Title)).ToList();
                if (!string.IsNullOrEmpty(model.Description))
                    list = list.Where(a => a.description.Contains(model.Description)).ToList();
                list =
                    list.Where(
                        a =>
                            model.BeforePublishDate
                                ? DateTime.Compare(a.date, model.PublishDate) <= 0
                                : DateTime.Compare(a.date, model.PublishDate) >= 0).ToList();
                if (!string.IsNullOrEmpty(model.Author))
                    list = list.Where(a => GetUserName(a.user_id).Contains(model.Author)).ToList();
                //if (model.KeyWords != null && model.KeyWords.Length > 0)
                //    list = list.Where(a => a.keyWords.Contains(model.KeyWords)).ToList();
                if (model.HasImages)
                    list = list.Where(HasImages).ToList();
                if (model.HasDocuments)
                    list = list.Where(HasDocuments).ToList();

                model.Articles = list;
                return View(model);
            }
            catch
            {
                return View(new SearchViewModels());
            }
        }

        public List<Article> SearchArticles(string str)
        {
            try
            {
                var res = Context.Articles.Where(a => a.description.Contains(str) || a.name.Contains(str));
                return res.ToList();
            }
            catch
            {
                return new List<Article>();
            }
        }

        public List<Article> AdvancedSearch(SearchViewModels model)
        {
            try
            {
                var list = Context.Articles.ToList();

                if (!string.IsNullOrEmpty(model.Title))
                    list = list.Where(a => a.name.Contains(model.Title)).ToList();
                if (!string.IsNullOrEmpty(model.Description))
                    list = list.Where(a => a.description.Contains(model.Description)).ToList();
                list =
                    list.Where(
                        a =>
                            model.BeforePublishDate
                                ? DateTime.Compare(a.date, model.PublishDate) <= 0
                                : DateTime.Compare(a.date, model.PublishDate) >= 0).ToList();
                if (!string.IsNullOrEmpty(model.Author))
                    list = list.Where(a => GetUserName(a.user_id).Contains(model.Author)).ToList();
                //if (model.KeyWords != null && model.KeyWords.Length > 0)
                //    list = list.Where(a => a.keyWords.Contains(model.KeyWords)).ToList();
                if (model.HasImages)
                    list = list.Where(HasImages).ToList();
                if (model.HasDocuments)
                    list = list.Where(HasDocuments).ToList();

                return list;
            }
            catch
            {
                return new List<Article>();
            }
        }

        private bool HasImages(Article a)
        {
            return HasX(a, true);
        }
 

        private bool HasDocuments(Article a)
        {
            return HasX(a, false);
        }

        private bool HasX(Article a, bool images)
        {
            try
            {
                var elements = Context.Elements.Where(e => e.article_id == a.id).ToList();
                return elements.Any(elt => elt.type == (images ? 1 : 2));
            }
            catch
            {
                return false;
            }
        }

        private string GetUserName(string id)
        {
            try
            {
                var user = UserManager.Users.First(u => u.Id == id);
                return user == null ? "" : user.UserName;
            }
            catch
            {
                return "";
            }
        }
    }
}
