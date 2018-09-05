using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using accountapi.Models;
using accountapi.Repository;

namespace accountapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountControler : ControllerBase
    {
        private readonly AccountContent _context;

        public AccountControler(AccountContent context)
        {
            _context = context;
        }

        [HttpGet]
        public User GetUserByid(int id)
        {
            return _context.Users.First(p => p.UserId == id);
        }
        
    }
}