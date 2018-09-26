using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Models.API
{
    public class ApiResult
    {
        public int status  { get; set; }

        public object result;

        public ApiResult(int _status=200)
        {
            status = _status;
        }

    }
}
