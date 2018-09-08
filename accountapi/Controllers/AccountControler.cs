using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using accountapi.Models;
using accountapi.Repository;
using Akka.Actor;
using accountapi.Contents;
using accountapi.Actors;

namespace accountapi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountControler : ControllerBase
    {
        private readonly AccountContent _context;
        private readonly AccountService _service;
        private readonly ActorSystem _actorSystem;
       

        public AccountControler(AccountContent context, 
            ActorSystem actorSystem,
            AccountService accountService)
        {
            _context = context;
            _service = accountService;
            _actorSystem = actorSystem;           
        }



        [HttpGet("user/{id}")]
        public User GetUserByid(int id)
        {            
            return _service.GetUserByid(id);
        }

        [HttpGet("sysinfo/akka")]
        public String GetActorSystemInfo()
        {
            return _service.GetActorSystemInfo();
        }

    }
}