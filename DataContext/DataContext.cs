using LibraryApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<LibraryTable> LibraryTable { get; set; }
        public DbSet<ImageTable> ImageTable { get; set; }
        public DbSet<LibraryLog> LibraryLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LibraryTable>()
                .HasKey(e => e.Id);

       
            modelBuilder.Entity<ImageTable>()
                .HasKey(e => e.ImageId); 

            modelBuilder.Entity<LibraryLog>()
                .HasKey(e => e.LogId);

        }

    }
}
