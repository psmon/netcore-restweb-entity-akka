using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Models.API
{
    public class RegisterReq
    {
        public String NickName { get; set; }
        
        public String MyId { get; set; }
        
        public String PassWord { get; set; }
    }
}
