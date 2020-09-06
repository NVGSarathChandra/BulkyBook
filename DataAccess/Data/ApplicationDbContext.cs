using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.IServiceContracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Bulky_Book_Project.Dataaccess.data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> categories{ get; set; }
        public DbSet<CoverType> coverTypes { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
