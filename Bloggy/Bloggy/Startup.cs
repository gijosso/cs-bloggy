using System.Linq;
using System.Web;
using Bloggy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bloggy.Startup))]
namespace Bloggy
{
    public partial class Startup
    {
        private static readonly ApplicationDbContext Context = new ApplicationDbContext();

        private readonly RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Context));
        private readonly ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(Context));


        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SetupRoles();
            SetupAdminAccount();
            SetupFRLanguage();
            SetupENLanguage();
        }

        private void SetupRoles()
        {
            try
            {
                //Admin role setup
                if (!_roleManager.RoleExists("Admin"))
                {
                    _roleManager.Create(new IdentityRole {Name = "Admin"});
                }

                //Translator role setup   
                if (!_roleManager.RoleExists("Translator"))
                {
                    _roleManager.Create(new IdentityRole {Name = "Translator"});
                }

                //User role setup   
                if (_roleManager.RoleExists("User")) return;
                {
                    _roleManager.Create(new IdentityRole {Name = "User"});
                }
            }
            catch
            {
                
            }
        }

        private void SetupAdminAccount()
        {
            try
            {
                if (_roleManager.RoleExists("Admin") && _userManager.Users.ToList().Count == 0)
                {
                    //Create a first user               
                    var user = new ApplicationUser
                    {
                        UserName = "admin@bloggy.com",
                        Email = "admin@bloggy.com",
                        LockoutEnabled = true
                    };
                    const string userPassword = "P@ssw0rd!";
                    var checkUser = _userManager.Create(user, userPassword);

                    //Upgrade 'admin' to Admin role
                    if (checkUser.Succeeded)
                    {
                        _userManager.AddToRole(user.Id, "Admin");
                    }
                }
            }
            catch
            {
                
            }
        }

        private void SetupFRLanguage()
        {
            try
            {
                var lang = Context.LangList.ToList();
                if (lang.Any() && lang.FirstOrDefault(x => x.lang == "fr-FR") != null)
                    return;
                var model = new LangList
                {
                    id = 0,
                    lang = "fr-FR",
                    name = "Français"
                };
                Context.LangList.Add(model);
                Context.SaveChanges();
            }
            catch
            {
                
            }
        }

        private void SetupENLanguage()
        {
            try
            {
                var lang = Context.LangList.ToList();
                if (lang.Any() && lang.FirstOrDefault(x => x.lang == "en-EN") != null)
                    return;
                var model = new LangList
                {
                    id = 1,
                    lang = "en-EN",
                    name = "English"
                };
                Context.LangList.Add(model);
                Context.SaveChanges();
            }
            catch
            {
                
            }
        }
    }
}
