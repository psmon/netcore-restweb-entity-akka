using accountapi.Models;
using Microsoft.EntityFrameworkCore;

namespace accountapi.Contents
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
            //관계설정
            modelBuilder.Entity<TokenHistory>()                
                .HasOne(t => t.User)
                .WithMany(b => b.TokenHistorys)
                .OnDelete(DeleteBehavior.Cascade);
            

            //인덱스 설정
            modelBuilder.Entity<TokenHistory>()
                .HasIndex(t => new { t.AuthToken })
                .IsUnique(true);
        }
        
    }
}
