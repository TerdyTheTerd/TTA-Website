using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ForumsDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        

        public ForumsDbContext() : base("DefaultConnection")
        {
        }


    }
}