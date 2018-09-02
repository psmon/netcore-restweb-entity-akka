using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accountapi.Models;
using accountapi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace accountapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly AccountContent _context;

        public ValuesController( AccountContent context )
        {
            _context = context;

            if ( _context.Students.Count() == 0 )
            {
                // 테스트용
                // Create a new Student if collection is empty,
                // which means you can't delete all Student.
                _context.Students.Add(new Student { ID=0, FirstName = "ORM",LastName="Entity" });
                _context.SaveChanges();
            }
        }


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get( int id )
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post( [FromBody] string value )
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put( int id, [FromBody] string value )
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete( int id )
        {
        }
    }
}
