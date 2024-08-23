using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NonogramCore.Entity;

namespace NonogramCore.Service
{
    public class NonogramDbContext : DbContext
    {
        public DbSet<Score> Scores { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Nonogram;Trusted_Connection=True;");
        }

    }
}
