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

        /// <summary>
        /// 로그인후 자신의 계정처리에 필요한 accessToken을 할당받습니다.
        /// </summary>
        /// <param name="user"></param>   
        [HttpPost("login")]
        public ActionResult<LoginRes> PostUser([FromBody] LoginReq user)
        {
            
            if (!ModelState.IsValid)
            {                
                return BadRequest(ModelState);
            }
            String[] tokenAndNick = _service.GetAccessToken(user.Id, user.Pw).Split("^^");

            LoginRes result = new LoginRes();
            result.accessToken = tokenAndNick[0];
            result.nick = tokenAndNick[1];

            return Ok(result);
            
        }

        /// <summary>
        /// accessToken을 통해 자신의 정보 조회를 합니다.
        /// </summary>
        /// <param name="accessToken"></param>
        [HttpGet("myinfo/{accessToken}")]
        public ActionResult<User> GetUserByid(String accessToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_service.GetMyInfo(accessToken));
        }

    }
}