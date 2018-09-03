using accountapi.Models;
using Microsoft.EntityFrameworkCore;

namespace accountapi.Repository
{
    
    public class AccountContent : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public AccountContent( DbContextOptions<AccountContent> options )
        : base(options)
        {
            
        }


    }
}
