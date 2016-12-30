using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.IdentityExtensions
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager() : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
            PasswordValidator = new CustomPasswordValidator(8);
        }
    }
}