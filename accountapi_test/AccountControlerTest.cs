using accountapi.Controllers;
using accountapi.Models;
using accountapi.Repository;
using accountapi.Contents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;
using Akka.Actor;
using accountapi.Actors;
using accountapi.Models.Test;
using System.Threading.Tasks;
using accountapi.Config;
using System.IO;
using System.Diagnostics;
using Xunit.Sdk;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace accountapi_test
{

    public class AccountControlerTest
    {
        public static readonly string ConnectionString = "server=localhost;database=db_account;user=psmon;password=db1234";

        public static readonly string testConnectionString = "server=localhost;database=db_test;user=psmon;password=db1234";
        
        public AccountControlerTest()
        {
            InitContext();
        }

        private AccountContent _context;
        private ActorSystem _actorSystem;
        private AccountService _accountService;
        private TestContext _testContext;
        private TestContext _testContext2;

        internal static int PrepareTestData()
        {
            var builder = new DbContextOptionsBuilder<AccountContent>()
                .UseLoggerFactory(LogSettings.DebugLogger)
                .UseMySql(AccountControlerTest.ConnectionString);
            var context = new AccountContent(builder.Options);
            

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var users = Enumerable.Range(1, 10)
                .Select(i => new User { MyId = "TestID" + i, PassWord = "TEST123" + i, NickName = "Mynick" + i, RegDate = DateTime.Now, IsSocialActive = false });

            context.Users.AddRange(users);
            context.SaveChanges();

            var testOpt = new DbContextOptionsBuilder<TestContext>()
                .UseLoggerFactory(LogSettings.DebugLogger)
                .UseMySql(AccountControlerTest.testConnectionString);

            var testContext = new TestContext(testOpt.Options);

            testContext.Database.EnsureDeleted();           
            testContext.Database.EnsureCreated();
            

            return context.Users.Count(t => t.IsSocialActive == false);
        }

        protected void InitContext()
        {
            var builder = new DbContextOptionsBuilder<AccountContent>()
                .UseLoggerFactory(LogSettings.DebugLogger)
                .UseMySql(AccountControlerTest.ConnectionString);

            _context = new AccountContent(builder.Options);
            _actorSystem = ActorSystem.Create("accountapi");
            _accountService = new AccountService(_context, _actorSystem);

            //For Test
            var testOpt = new DbContextOptionsBuilder<TestContext>()
                .UseLoggerFactory(LogSettings.DebugLogger)
                .UseMySql(AccountControlerTest.testConnectionString);

            _testContext = new TestContext(testOpt.Options);
            _testContext2 = new TestContext(testOpt.Options);

        }

        protected void ResetTestDB()
        {
            _testContext.Database.EnsureDeleted();
            _testContext.Database.EnsureCreated();
        }

        [Fact]
        public void ConcurrencyTest()
        {
            ResetTestDB();

            try
            {
                Person person1 = new Person();
                person1.LastName = "PS";
                person1.FirstName = "MON";
                _testContext.Persons.Add(person1);
                _testContext.SaveChanges();

                Person edit1 = _testContext2.Persons.FirstOrDefault(e => e.FirstName == "MON");
                Assert.Equal("PS", edit1.LastName);

                for (int i = 0; i < 10; i++)
                {
                    String editName = "PS" + i;
                    String editName2 = "XS" + i;
                    person1.LastName = editName;
                    edit1.LastName = editName2;
                    _testContext.SaveChangesAsync();
                    _testContext2.SaveChanges();
                }

            }
            catch(DbUpdateConcurrencyException e)
            {

            }
            
        }

        [Fact]
        public void Actor_CRUDTest()
        {
            _accountService.Create_CRUDActor("CrudActor");
           
            IActorRef crudActor = _accountService.GetLocalActor("CrudActor");

            crudActor.Tell(_accountService as ICurdRepo<User>);  //사용할 서비스 지정

            String testNick = "iam actor";

            CRUDAction res = crudActor.Ask(new CRUDAction()
            {
                action = "insert",
                data = new User() { MyId = "actor1", PassWord = "1234", NickName = testNick },
                isupdate = true

            }).Result as CRUDAction;

            User someUser = _context.Users.FirstOrDefault(u => u.NickName == testNick );

            Assert.Equal(testNick, someUser.NickName);
            
        }

        [Fact]
        public void TestUserCntChk()
        {
            var count = _context.Users.Count(t => t.IsSocialActive==false);
            
            Assert.InRange(count, 9, 20);
        }

        [Fact]
        public void TokenTest1()
        {
            var service = new AccountService(_context, _actorSystem);           
            var accessToken = service.GetAccessToken("TestID1", "TEST1231");
            User myInfo = service.GetMyInfo(accessToken);
            Assert.Equal("Mynick1", myInfo.NickName);           
            //TestID1 TEST1231
        }
        
        [Fact]
        public void TestUserActive()
        {
            bool expected = false;
            var controller = new AccountControler(_accountService);
            User result = controller.GetUserByid(1);
            Assert.Equal(expected, result.IsSocialActive);
        }

        [Fact]
        public void LocalStoage()
        {
            LocalRepository<LocalEntity<User>> localRepository = new LocalRepository<LocalEntity<User>>();

            for(int i = 0; i < 10; i++)
            {
                LocalEntity<User> addUser = new LocalEntity<User>()
                    { ID=i.ToString(),Data=new User() { NickName="name-"+i.ToString()  } };

                localRepository.AddObj(addUser);
            }

            String nickName =localRepository.GetObj("5").Data.NickName;
            Assert.Equal("name-5", nickName);
            
        }


    }
}
