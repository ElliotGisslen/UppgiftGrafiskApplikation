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
            Database.EnsureCreated();
            try
            {
                Database.Migrate();
            }
            catch { }
        }

        public SmartAppDbContext(DbContextOptions<SmartAppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

        public DbSet<SmartAppSettings> Settings { get; set; }
    }
}
