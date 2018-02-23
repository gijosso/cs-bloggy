using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bloggy.Models
{
    public class AdminViewModels
    {
        public IEnumerable<AdminUserViewModel> Users { get; set; }
        public IEnumerable<LangList> LangList { get; set; }
    }

    public class AdminUserViewModel
    {
        [Key]
        public ApplicationUser ApplicationUser { get; set; }
        public IdentityRole IdentityRole { get; set; }
    }
}