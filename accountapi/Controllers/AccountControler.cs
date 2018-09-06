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
        private readonly AccountService _service;

        public AccountControler(AccountContent context)
        {
            _context = context;
            _service = new AccountService(_context);
        }

        [HttpGet]
        public User GetUserByid(int id)
        {            
            return _service.GetUserByid(id);
        }
        
    }
}