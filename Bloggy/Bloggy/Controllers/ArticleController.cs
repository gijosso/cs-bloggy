using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bloggy.Models;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using System.Net;


namespace Bloggy.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ArticleController : Controller
    {
        private ApplicationDbContext _context;

        public ArticleController()
        {

        }

        public ArticleController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        public ActionResult Read(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var article = Context.Articles.FirstOrDefault(x => x.id == id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                var author = Context.Users.FirstOrDefault(x => x.Id == article.user_id);
                var elements = Context.Elements.Where(x => x.article_id == article.id);
                article.views += 1;
                Context.Entry(article).State = EntityState.Modified;
                Context.SaveChanges();
                return View(new UserArticleViewModel
                {
                    Article = article,
                    Author = author,
                    Elements = elements
                });
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}