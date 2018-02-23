using Bloggy.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bloggy.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
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

        public ActionResult Index()
        {
            try
            {
                var articles = Context.Articles.Where(x => x.validated).OrderByDescending(x => x.views).Take(10).ToList();
                var langList = Context.LangList.ToList();
                return View(new HomeViewModel
                {
                    Articles = articles,
                    LangList = langList
                });
            }
            catch (Exception)
            {
                return View(new HomeViewModel());
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
