using Microsoft.EntityFrameworkCore;
namespace MVC.Models
{
    public class EFCoreWebDemoContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Persist Security Info=True;Initial Catalog=EFCore;Data Source=MANDAARJ\SQL2017;User ID=sa;Password=cybage@123;");
        }
    }
}
