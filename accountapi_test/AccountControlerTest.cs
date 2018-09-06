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
        public static readonly string ConnectionString = "server=localhost;database=db_account;user=psmon;password=db1234";

        public AccountControlerTest()
        {
            InitContext();
        }

        private AccountContent _context;

        internal static int PrepareTestData()
        {
            var builder = new DbContextOptionsBuilder<AccountContent>()
                .UseMySql(AccountControlerTest.ConnectionString);
            var context = new AccountContent(builder.Options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var users = Enumerable.Range(1, 10)
                .Select(i => new User { MyId = "TestID" + i, PassWord = "TEST123" + i, NickName = "Mynick" + i, RegDate = DateTime.Now, IsSocialActive = false });

            context.Users.AddRange(users);
            context.SaveChanges();           
            return context.Users.Count(t => t.IsSocialActive == false);
        }

        protected void InitContext()
        {
            var builder = new DbContextOptionsBuilder<AccountContent>()
                .UseMySql(AccountControlerTest.ConnectionString);

            var context = new AccountContent(builder.Options);            
            _context = context;            
        }

        [Fact]
        public void TestUserCntChk()
        {
            var count = _context.Users.Count(t => t.IsSocialActive==false);
            Assert.Equal(10, count);
        }

        [Fact]
        public void TokenTest1()
        {
            var service = new AccountService(_context);           
            var accessToken = service.GetAccessToken("TestID1", "TEST1231");
            User myInfo = service.GetMyInfo(accessToken);
            Assert.Equal("Mynick1", myInfo.NickName);           
            //TestID1 TEST1231
        }
        
        [Fact]
        public void TestUserActive()
        {
            bool expected = false;
            var controller = new AccountControler(_context);
            User result = controller.GetUserByid(1);
            Assert.Equal(expected, result.IsSocialActive);
        }
    }
}
