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
using accountapi.Models.API;

namespace accountapi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountControler : ControllerBase
    {
       
        private readonly AccountService _service;
       
        public AccountControler(AccountService accountService)
        {            
            _service = accountService;                      
        }

        
        [HttpGet("user/{id}")]
        public User GetUserByid(int id)
        {            
            return _service.GetUserByid(id);
        }

        [HttpGet("sysinfo/akka")]
        public String GetActorSystemInfo()
        {            
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("akkainfo")))
            {
                String systemInfo = _service.GetActorSystemInfo();
                HttpContext.Session.SetString( "akkainfo", String.Format("{0} == {1}", systemInfo,DateTime.Now) );
            }

            var systemInfo_cache = HttpContext.Session.GetString("akkainfo");

            return systemInfo_cache;
        }

        // POST: api/AccountControler
        [HttpPost("login")]
        public ApiResult PostUser([FromBody] LoginReq user)
        {
            if (!ModelState.IsValid)
            {                
                return new ApiResult(StatusCodes.Status400BadRequest);
            }
            String accessToken = _service.GetAccessToken(user.Id, user.Pw);

            return new ApiResult {
                result = new LoginRes(){
                    accessToken = accessToken 
                }
            };
        }

        [HttpGet("myinfo/{accessToken}")]
        public User GetUserByid(String accessToken)
        {
            return _service.GetMyInfo(accessToken);
        }

    }
}