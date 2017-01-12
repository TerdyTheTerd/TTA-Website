using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    public class BaseController : Controller
    {
        
        protected IApplicationDbContext db;
        public BaseController()
        {
            db = new ApplicationDbContext();
        }
        public BaseController(IApplicationDbContext dbContext)
        {
            db = dbContext;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}