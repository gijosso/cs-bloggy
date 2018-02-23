using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Bloggy.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    [Table("article")]
    public class Article
    {
        [Key]
        public Int64 id { get; set; }
        public string lang { get; set; }
        public string name { get; set; }
        public string banner { get; set; }
        public string description { get; set; }
        public Int64 post_id { get; set; }
        public string user_id { get; set; }
        public bool validated { get; set; }
        public DateTime date { get; set; }
        public Int64 views { get; set; }
    }

    [Table("element")]
    public class Element
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 id { get; set; }
        public Int64 article_id { get; set; }
        public Int64 type { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public Int64 index { get; set; }
    }

    [Table("lang_list")]
    public class LangList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 id { get; set; }
        public string lang { get; set; }
        public string name { get; set; }

    }

    [Table("post")]
    public class Post
    {
        [Key]
        public Int64 id { get; set; }
        public String name { get; set; }
    }
  
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<LangList> LangList { get; set; }
    }
}
