using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataAccess.Models;

namespace WebApi.DataAccess.Data
{
    public class FormulaContext : IdentityDbContext<ApplicationUser>
    {
        public FormulaContext(DbContextOptions<FormulaContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>().HasIndex(p => p.Id);
            modelBuilder.Entity<Team>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<ApplicationUser>().HasIndex(p => new { p.UserName });
        }

        public DbSet<Team> Teams { get; set; }
    }
}
