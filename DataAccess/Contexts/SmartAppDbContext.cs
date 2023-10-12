using DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public class SmartAppDbContext : DbContext
    {

        public SmartAppDbContext()
        {
        }

        public SmartAppDbContext(DbContextOptions<SmartAppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            try
            {
                Database.Migrate();
            }
            catch { }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

        public DbSet<SmartAppSettings> Settings { get; set; }
    }
}
