using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Bloggy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;

namespace Bloggy.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "Admin, Translator")]
    public class PostController : Controller
    {
        private ApplicationDbContext _context;

        public PostController()
        {

        }

        public PostController(ApplicationDbContext context)
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

        // GET: Post
        public ActionResult Index(PostMessageId? message)
        {
            ViewBag.StatusMessage =
                message == PostMessageId.EditSuccess ? "Your changes have been applied."
                : message == PostMessageId.AsyncError ? "Changes can't be applied to the database. Try again later."
                : message == PostMessageId.NoAction ? "No action performed."
                : message == PostMessageId.Error ? "An error has occurred."
                : "";

            var postsModel = new PostsViewModel
            {
                Posts = Context.Posts,
            };
            return View(postsModel);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                Context.Posts.Add(model);
                Context.SaveChanges();
                
                return RedirectToAction("Index", new { Message = PostMessageId.CreateSuccess });
            }
            catch
            {
                return RedirectToAction("Index", new { Message = PostMessageId.Error });
            }
        }

        // GET: Post/Edit
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var post = Context.Posts.FirstOrDefault(x => x.id == id);
                if (post == null)
                {
                    return HttpNotFound();
                }
                var model = new PostViewModel
                {
                    Post = post,
                    Articles = Context.Articles.Where(x => x.post_id == id).ToList()
                };
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.InnerException);
                return RedirectToAction("Index");
            }
        }

        // POST: Post/Edit
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PostViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                Context.Entry(model.Post).State = EntityState.Modified;
                Context.SaveChanges();

                return RedirectToAction("Edit/" + model.Post.id, new { Message = PostMessageId.EditSuccess });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return RedirectToAction("Index", new { Message = PostMessageId.Error });
            }
        }

        // GET: Post/AddArticle/x
        public ActionResult AddArticle(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (!Context.Posts.Any(x => x.id == id))
                {
                    return HttpNotFound();
                }
                var context = new ApplicationDbContext();
                var postArticlesLangCode = context.Articles.Where(x => x.post_id == id).Select(x => x.lang);
                var langs = context.LangList.ToList();
                var postLang = langs.Where(lang => !postArticlesLangCode.Any(code => code == lang.lang));
                var langLists = postLang as IList<LangList> ?? postLang.ToList();
                return View(new AddArticleViewModel
                {
                    Article = new Article { id = (long)id },
                    LangList = langLists
                });
            }
            catch
            {
                return View(new AddArticleViewModel
                {
                    Article = new Article(),
                    LangList = new List<LangList>()
                });
            }
        }

        // POST: Post/AddArticle/x
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddArticle(long? id, HttpPostedFileBase file, [Bind(Exclude = "post_id, user_id, validated, date, views, banner")]AddArticleViewModel model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                if (!Context.Posts.Any(x => x.id == id))
                {
                    return HttpNotFound();
                }

                if (!ModelState.IsValid)
                {
                    Console.WriteLine(model);
                    return View(model);
                }

                model.Article.post_id = (long) id;

                model.Article.id = 0;
                model.Article.user_id = User.Identity.GetUserId();
                model.Article.date = DateTime.Now;
                if (file != null)
                {
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png" };
                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                        TempData["error"] = ("Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }
                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + (MaxContentLength / (1024 * 1024)) + " MB");
                    }
                    else
                    {
                        string extension = Path.GetExtension(file.FileName);
                        //string fileName = DateTime.Now.ToString("ddMMyyHHmmss").ToString() + extension;
                        string filename = Guid.NewGuid() + extension;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Image"), filename));
                        ModelState.Clear();
                        ViewBag.Message = "File uploaded successfully";
                        model.Article.banner = "/Content/Image/" + filename;
                    }
                }
                else
                {
                    model.Article.banner = "";
                }

                //check for SQL stuff
                Context.Articles.Add(model.Article);
                Context.SaveChanges();

                return RedirectToAction("Edit/" + model.Article.post_id, new { Message = PostMessageId.CreateSuccess });
            }
            catch
            {
                return RedirectToAction("Edit/" + model.Article.post_id, new { Message = PostMessageId.Error });
            }
        }

        // GET: Post/Article/x (edition)
        public ActionResult Article(long? id)
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
            return View(new ArticleViewModel
            {
                Article = article,
                Elements = Context.Elements.Where(x => x.article_id == id)
            });
        }

        // POST: Post/Article/x (edition)
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Article(HttpPostedFileBase file, ArticleViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine(model);
                    return View(model);
                }
                if (file != null)
                {
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png" };
                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                        TempData["error"] = ("Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }
                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + (MaxContentLength / (1024 * 1024)) + " MB");
                    }
                    else
                    {
                        string extension = Path.GetExtension(file.FileName);
                        //string fileName = DateTime.Now.ToString("ddMMyyHHmmss").ToString() + extension;
                        string filename = Guid.NewGuid() + extension;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Image"), filename));
                        ModelState.Clear();
                        ViewBag.Message = "File uploaded successfully";
                        model.Article.banner = "/Content/Image/" + filename;
                    }
                }
                model.Article.date = DateTime.Now;
                Context.Entry(model.Article).State = EntityState.Modified;
                Context.SaveChanges();

                return RedirectToAction("Article/" + model.Article.id, new { Message = PostMessageId.EditSuccess });
            }
            catch
            {
                return RedirectToAction("Index", new { Message = PostMessageId.Error });
            }
        }

        // GET: Post/AddElement/x
        public ActionResult AddElement(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (!Context.Articles.Any(x => x.id == id))
                {
                    return HttpNotFound();
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // POST: Post/AddElement/x
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddElement(long? id, HttpPostedFileBase file, [Bind(Exclude = "article_id, index")]Element model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                if (!Context.Articles.Any(x => x.id == id))
                {
                    return HttpNotFound();
                }

                if (!ModelState.IsValid)
                {
                    Console.WriteLine(model);
                    return View(model);
                }

                model.article_id = (long) id;

                var articleElements = Context.Elements.Where(x => x.article_id == model.article_id);
                var elements = Context.Elements;

                model.id = elements.Any() ? elements.Max(x => x.id) + 1 : (long) 0;
                model.index = articleElements.Any() ? articleElements.Max(x => x.index) + 1 : (long) 0;

                //model.user_id = User.Identity.GetUserId();

                if (model.type == 1)
                {
                    if (file == null)
                        return View(model);
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png" };
                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                        TempData["error"] = ("Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }
                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + (MaxContentLength / (1024 * 1024)) + " MB");
                    }
                    else
                    {
                        string extension = Path.GetExtension(file.FileName);
                        //string fileName = DateTime.Now.ToString("ddMMyyHHmmss").ToString() + extension;
                        string filename = Guid.NewGuid() + extension;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Image"), filename));
                        ModelState.Clear();
                        ViewBag.Message = "File uploaded successfully";
                        model.link = "/Content/Image/" + filename;
                    }
                }
                else if (model.type == 2)
                {
                    if (file == null)
                        return View(model);
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".pdf", ".docx" };
                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                        TempData["error"] = ("Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }
                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + (MaxContentLength / (1024 * 1024)) + " MB");
                    }
                    else
                    {
                        string extension = Path.GetExtension(file.FileName);
                        //string fileName = DateTime.Now.ToString("ddMMyyHHmmss").ToString() + extension;
                        string filename = Guid.NewGuid() + extension;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Document"), filename));
                        ModelState.Clear();
                        ViewBag.Message = "File uploaded successfully";
                        model.link = "/Content/Document/" + filename;
                    }
                }
                else
                {

                }

                //check for SQL stuff
                Context.Elements.Add(model);
                Context.SaveChanges();

                return RedirectToAction("Article/" + model.article_id, new { Message = PostMessageId.CreateSuccess });
            }
            catch
            {
                return RedirectToAction("Article/" + model.article_id, new { Message = PostMessageId.Error });
            }
        }

        public static List<SelectListItem> ProvideAvailableLangList(long id)
        {
            try
            {
                var context = new ApplicationDbContext();
                var postArticlesLangCode = context.Articles.Where(x => x.post_id == id).Select(x => x.lang);
                var langs = context.LangList.ToList();
                var postLang = langs.Where(lang => !postArticlesLangCode.Any(code => code == lang.lang));
                var langLists = postLang as IList<LangList> ?? postLang.ToList();
                var ret = new List<SelectListItem>();
                if (!langLists.Any())
                {
                    ret.Add(new SelectListItem
                    {
                        Text = "No available language for this article",
                        Value = null,
                        Selected = true
                    });
                }
                else
                {
                    ret.AddRange(langLists.Select(lang => new SelectListItem
                    {
                        Text = lang.name,
                        Value = lang.lang
                    }));
                }
                return ret;
            }
            catch (Exception)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "No available language for this article",
                        Value = null,
                        Selected = true
                    }
                };
            }
        }

        // GET: Post/Delete/
        public ActionResult DeletePost(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Post post = Context.Posts.Find(id);
                if (post == null)
                {
                    return HttpNotFound();
                }
                return View(post);
            }
            catch
            {
                return RedirectToAction("", new { Message = PostMessageId.Error });
            }
        }

        // POST: Post/Delete/
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedPost(long id)
        {
            try
            {
                Post post = Context.Posts.Find(id);
                var ats = Context.Articles.Where(x => x.post_id == id);
                foreach (var article in ats)
                {
                    var els = Context.Elements.Where(x => x.article_id == article.id);
                    foreach (var item in els)
                    {
                        Context.Elements.Remove(item);
                    }
                    Context.Articles.Remove(article);
                }
                Context.Posts.Remove(post);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("", new { Message = PostMessageId.Error });
            }
        }

        // GET: Articles/Delete/
        public ActionResult DeleteArticle(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Article article = Context.Articles.Find(id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                return View(article);
            }
            catch
            {
                return RedirectToAction("", new { Message = PostMessageId.Error });
            }
        }

        // POST: Articles/Delete/
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("DeleteArticle")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedArticle(long id)
        {
            try
            {
                Article article = Context.Articles.Find(id);
                var pid = article.post_id;
                var els = Context.Elements.Where(x => x.article_id == id);
                Context.Articles.Remove(article);
                foreach (var item in els)
                {
                    Context.Elements.Remove(item);
                }
                Context.SaveChanges();
                return RedirectToAction("Edit/" + pid);
            }
            catch
            {
                return RedirectToAction("", new { Message = PostMessageId.Error });
            }
        }

        // GET: Element/Delete/
        public ActionResult DeleteElement(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Element element = Context.Elements.Find(id);
                if (element == null)
                {
                    return HttpNotFound();
                }
                return View(element);
            }
            catch
            {
                return RedirectToAction("", new { Message = PostMessageId.Error });
            }
        }

        // POST: Element/Delete/
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("DeleteElement")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedElement(long id)
        {
            try
            {
                Element element = Context.Elements.Find(id);
                var aid = element.article_id;
                Context.Elements.Remove(element);
                Context.SaveChanges();
                return RedirectToAction("Article/" + aid);
            }
            catch
            {
                return RedirectToAction("", new { Message = PostMessageId.Error });
            }
        }

        // GET: Post/Article/x (edition)
        public ActionResult Element(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var element = Context.Elements.FirstOrDefault(x => x.id == id);
            if (element == null)
            {
                return HttpNotFound();
            }
            return View(element);
        }

        // POST: Post/Article/x (edition)
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Element(HttpPostedFileBase file, Element model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine(model);
                    return View(model);
                }
                if (file != null)
                {
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png" };
                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                        TempData["error"] = ("Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }
                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + (MaxContentLength / (1024 * 1024)) + " MB");
                    }
                    else
                    {
                        string extension = Path.GetExtension(file.FileName);
                        //string fileName = DateTime.Now.ToString("ddMMyyHHmmss").ToString() + extension;
                        string filename = Guid.NewGuid() + extension;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Image"), filename));
                        ModelState.Clear();
                        ViewBag.Message = "File uploaded successfully";
                        model.link = "/Content/Image/" + filename;
                    }
                }

                Context.Entry(model).State = EntityState.Modified;
                Context.SaveChanges();

                return RedirectToAction("Article/" + model.article_id, new { Message = PostMessageId.EditSuccess });
            }
            catch
            {
                return RedirectToAction("Index", new { Message = PostMessageId.Error });
            }
        }

        public enum PostMessageId
        {
            NoAction,
            CreateSuccess,
            EditSuccess,
            AsyncError,
            ArticleLanguageExists,
            Error
        }
    }
}
