using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using accountapi.Models.Test;

namespace accountapi.Contents
{
    public class TestContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Person> Persons { get; set; }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
