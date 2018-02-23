using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Bloggy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Bloggy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            RoleManager = roleManager;
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

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Admin
        public ActionResult Index(AdminMessageId? message)
        {
            ViewBag.StatusMessage =
                message == AdminMessageId.EditSuccess ? "Your changes have been applied."
                : message == AdminMessageId.AsyncError ? "Changes can't be applied to the database. Try again later."
                : message == AdminMessageId.NoAction ? "No action performed."
                : message == AdminMessageId.Error ? "An error has occurred."
                : "";

            try
            {
                var users = UserManager.Users.ToList();
                var usersModel = users.Select(x => new AdminUserViewModel
                {
                    ApplicationUser = x,
                    IdentityRole = x.Roles.Count > 0 ? RoleManager.FindById(x.Roles.First().RoleId) : null
                }).ToList();
                var langListModel = Context.LangList.ToList();
                return View(new AdminViewModels {Users = usersModel, LangList = langListModel});
            }
            catch (Exception)
            {
                return View(new AdminViewModels());
            }
        }

       // GET: Admin/Edit/5
        public async Task<ActionResult> EditUser(string id)
        {
            try
            {
                var user = UserManager.Users.Where(x => x.Id == id);

                var au = await user.FirstOrDefaultAsync();
                var userModel = new AdminUserViewModel();
                if (au == null) return RedirectToAction("Index");
                userModel.ApplicationUser = au;
                var task = au.Roles.Count > 0 ? RoleManager.FindByIdAsync(au.Roles.First().RoleId) : null;
                if (task != null)
                    userModel.IdentityRole = await task;
                return View(userModel);
            }
            catch (Exception)
            {
                return View("Index");
            }
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(string id, AdminUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await UserManager.FindByIdAsync(id);
                var emailChanged = user.Email != model.ApplicationUser.Email;
                IdentityResult result = null;
                if (emailChanged)
                {
                    user.Email = model.ApplicationUser.Email;
                    user.UserName = model.ApplicationUser.Email;
                    result = await UserManager.UpdateAsync(user);
                }
                if (!emailChanged || result.Succeeded)
                {
                    var identityUserRole = user.Roles.FirstOrDefault();
                    if (identityUserRole != null)
                    {
                        var role = await RoleManager.FindByIdAsync(identityUserRole.RoleId);
                        if (role.Name != model.IdentityRole.Name)
                        {
                            var upgrade = await UserManager.AddToRoleAsync(user.Id, model.IdentityRole.Name);
                            if (upgrade.Succeeded)
                            {
                                var demote = await UserManager.RemoveFromRolesAsync(user.Id, role.Name);
                                if (demote.Succeeded)
                                {

                                }
                                else
                                {
                                    return RedirectToAction("Index", new { Message = AdminMessageId.AsyncError });
                                }
                            }
                            else
                            {
                                return RedirectToAction("Index", new { Message = AdminMessageId.AsyncError });
                            }
                        }
                        else if (!emailChanged)
                        {
                            return RedirectToAction("Index", new {Message = AdminMessageId.NoAction});
                        }
                    }
                    return RedirectToAction("Index", new {Message = AdminMessageId.EditSuccess});
                }
                return RedirectToAction("Index", new { Message = AdminMessageId.AsyncError });
            }
            catch
            {
                return RedirectToAction("Index", new { Message = AdminMessageId.Error });
            }
        }

        // GET: User/Delete/
        public ActionResult DeleteUser(string id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = UserManager.FindById(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch
            {
                return RedirectToAction("Index", new { Message = AdminMessageId.Error });
            }
        }

        //// POST: User/Delete/
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedUser(string id)
        {
            try
            {
                var user = UserManager.FindById(id);
                UserManager.Delete(user);
                return RedirectToAction("Index", new { Message = AdminMessageId.EditSuccess });
            }
            catch
            {
                return RedirectToAction("Index", new { Message = AdminMessageId.Error });
            }
        }

        // GET: Post/AddLanguage/x
        public ActionResult AddLanguage()
        {
            return View("AddLanguage");
        }

        // POST: Post/AddArticle/x
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLanguage(LangList model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine(model);
                    return View(model);
                }

                //check for SQL stuff
                var langList = Context.LangList.ToList();
                model.id = langList.Any() ? langList.Max(x => x.id) + 1 : (long) 1;

                Context.LangList.Add(model);
                Context.SaveChanges();

                return RedirectToAction("Index", new { Message = PostController.PostMessageId.CreateSuccess });
            }
            catch
            {
                return RedirectToAction("Index", new { Message = PostController.PostMessageId.Error });
            }
        }

        // GET: Admin/Delete/
        public ActionResult DeleteLang(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LangList post = Context.LangList.Find(id);
                if (post == null)
                {
                    return HttpNotFound();
                }
                return View(post);
            }
            catch
            {
                return RedirectToAction("Index", new { Message = AdminMessageId.Error });
            }
        }

        // POST: Admin/Delete/
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("DeleteLang")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedLang(long id)
        {
            try
            {
                LangList lang = Context.LangList.Find(id);
                Context.LangList.Remove(lang);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index", new { Message = AdminMessageId.Error });
            }
        }

        public enum AdminMessageId
        {
            NoAction,
            EditSuccess,
            AsyncError,
            Error
        }
    }
}