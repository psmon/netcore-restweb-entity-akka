using accountapi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accountapi.Models;
using accountapi.Contents;
using Akka.Actor;
using accountapi.Actors;


namespace accountapi.Controllers
{

    public class AccountService : ICurdRepo
    {
        private readonly AccountContent _context;
        private readonly ActorSystem _actorSystem;
        private readonly int WAITFOR_ACTOR_SEC = 3;

        public void Create_CRUDActor(string actorName)
        {
            _actorSystem.ActorOf<CRUDActor>(actorName);
        }

        public IActorRef GetLocalActor(string actorName)
        {
            return _actorSystem.ActorSelection("/user/" + actorName).ResolveOne(TimeSpan.FromSeconds(WAITFOR_ACTOR_SEC)).Result;
        }

        public AccountService(AccountContent context, ActorSystem actorSystem)
        {
            _context = context;
            _actorSystem = actorSystem;

            Console.WriteLine(_context.Users.First(p => p.UserId == 1).NickName);
            System.Console.WriteLine("Actor System Check===" + actorSystem.Name);
        }

        public String GetActorSystemInfo()
        {
            return String.Format("=== ActorSystem Info {0} {1}", _actorSystem.Name,_actorSystem.StartTime);
        }

        public User GetUserByid(int id)
        {
            return _context.Users.First(p => p.UserId == id);
        }

        public String GetAccessToken(string userid,string userpw)
        {
            User accessUser = _context.Users.First(p => p.MyId == userid && p.PassWord == userpw);
            if (accessUser == null)
            {
                throw new Exception("401");
            }
            else
            {
                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                _context.TokenHistories.Add(new TokenHistory()
                {
                    User = accessUser,
                    AuthToken = token,
                    CreateTime = DateTime.Now,
                    AccessTime = DateTime.Now
                });

                _context.SaveChanges();
                return token;
            }
        }

        public User GetMyInfo(string accessToken)
        {
            User myinfo= _context.TokenHistories.First(p => p.AuthToken == accessToken).User;
            if (myinfo == null) throw new Exception("401");
            myinfo.PassWord = "******";
            return myinfo;
        }

        public void AddUser(User user)
        {
            _context.Add(user);           
        }

        public void UpdateDB()
        {
            _context.SaveChanges();
        }

        public void DelUser(User user)
        {
            _context.Remove(user);
        }

        public User GetUser(string userID)
        {
            return _context.Users.Single( u=>u.MyId== userID);
        }
    }
}
