using accountapi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accountapi.Models;
using Akka.Actor;

namespace accountapi.Controllers
{
    public class AccountService
    {
        private readonly AccountContent _context;
        private readonly ActorSystem _actorSystem;

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

    }
}
