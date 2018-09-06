using accountapi.Models;
using Microsoft.EntityFrameworkCore;

namespace accountapi.Repository
{
    
    public class AccountContent : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SocialInfo> SocialInfos { get; set; }
        public DbSet<TokenHistory> TokenHistories { get; set; }

        public AccountContent( DbContextOptions<AccountContent> options )
        : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<TokenHistory>()
                .HasIndex(p => new { p.AuthToken })
                .IsUnique(true);*/
        }
        
    }
}
