using accountapi.Models;
using Microsoft.EntityFrameworkCore;

namespace accountapi.Repository
{
    
    public class AccountContent : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SocialInfo> SocialInfos { get; set; }

        public AccountContent( DbContextOptions<AccountContent> options )
        : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AuditEntry>();
        }


    }
}
