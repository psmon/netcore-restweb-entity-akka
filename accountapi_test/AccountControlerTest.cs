using accountapi.Controllers;
using accountapi.Models;
using accountapi.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace accountapi_test
{
    public class AccountControlerTest
    {
        public AccountControlerTest()
        {
            InitContext();
        }

        private AccountContent _context;

        protected void ResetContext(AccountContent _context)
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        protected void InitContext()
        {
            var builder = new DbContextOptionsBuilder<AccountContent>()
                .UseMySql("server=localhost;database=db_account;user=psmon;password=db1234");

            var context = new AccountContent(builder.Options);
            ResetContext(context);

            var users = Enumerable.Range(1, 10)
                .Select(i => new User { IsActive =false });

            context.Users.AddRange(users);
            int changed = context.SaveChanges();
            _context = context;
            
        }

        [Fact]
        public void TestUserActive()
        {
            bool expected = false;
            var controller = new AccountControler(_context);
            User result = controller.GetUserByid(1);
            Assert.Equal(expected, result.IsActive);
        }
    }
}
