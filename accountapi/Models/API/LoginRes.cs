using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Models.API
{
    public class LoginRes
    {
        public String nick { get; set; }
        public String accessToken { get; set; }
    }
}
