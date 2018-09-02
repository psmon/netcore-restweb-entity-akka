using accountapi.Models;
using Microsoft.EntityFrameworkCore;

namespace accountapi.Repository
{
    public class AccountContent : DbContext
    {
        public AccountContent( DbContextOptions<AccountContent> options )
        : base(options)
            { }
        

        /*
        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            
             // var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
             // optionsBuilder.UseSqlite("Data Source=blog.db");
             
            //optionsBuilder.UseSqlite("Data Source=blog.db");
            optionsBuilder.UseMySql("server=localhost;database=db_account;user=psmon;password=db1234");
        
        }
        */

        public DbSet<Student> Students { get; set; }
    }
}
