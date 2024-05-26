using _6.NovaPoshta.Constants;
using _6.NovaPoshta.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace _6.NovaPoshta.Data
{
    public class MyDataContext : DbContext
    {
        public DbSet<AreaEntity> Areas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(AppDatabase.ConnectionString);
        }
    }
}
