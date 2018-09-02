using accountapi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Repository
{
    public class StudentContext : DbContext
    {
        public StudentContext( DbContextOptions<StudentContext> options )
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
