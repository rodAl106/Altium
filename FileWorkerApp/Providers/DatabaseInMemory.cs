using FileWorkerApp.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace FileWorkerApp.Providers
{
    public class DatabaseInMemory : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "InMemorySample");
        }

        public DbSet<Manufacturer> Manufacturers { get; set; }

    }
}
