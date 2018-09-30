using accountapi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accountapi.Models;
using accountapi.Contents;
using Akka.Actor;
using accountapi.Actors;
using Microsoft.IdentityModel.Tokens;
using accountapi.Models.API;
using Microsoft.EntityFrameworkCore;

namespace accountapi.Controllers
{

    public class AccountService : ICurdRepo<User>
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

        public LoginRes GetAccessToken(string userid,string userpw)
        {
            LoginRes result = new LoginRes();
            User accessUser = _context.Users.First(p => p.MyId == userid && p.PassWord == userpw);
            if (accessUser == null)
            {
                throw new Exception("401");
            }
            else
            {
                string nick = accessUser.NickName;
                string base64 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                string token = Base64UrlEncoder.Encode(base64);
                
                _context.TokenHistories.Add(new TokenHistory()
                {
                    User = accessUser,
                    AuthToken = token,
                    CreateTime = DateTime.Now,
                    AccessTime = DateTime.Now
                });

                _context.SaveChanges();
                result.nick = nick;
                result.accessToken = token;

                return result;
            }
        }

        public User GetUserInfo(string accessToken)
        {
            User userInfo= _context.TokenHistories.Include( p=>p.User)
                .First(p => p.AuthToken.Equals(accessToken))
                .User;
            
            if (userInfo == null) throw new Exception("401");
            return userInfo;
        }

        public void AddObj(User user)
        {
            _context.Add(user);           
        }

        public void UpdateDB()
        {
            _context.SaveChanges();
        }

        protected void ClearChild(User user)
        {
            if (user.SocialInfos != null) user.SocialInfos.Clear();
            if (user.TokenHistorys != null) user.TokenHistorys.Clear();
        }

        public void DelObj(User user)
        {                        
            ClearChild(user);
            _context.Remove(user);
        }

        public User GetObj(string userID)
        {
            return _context.Users.Single( u=>u.MyId== userID);
        }
    }
}
