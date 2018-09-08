using accountapi.Controllers;
using accountapi.Models;
using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Actors
{
    public class CRUDAction
    {
        public string action { get; set; }
        public bool isupdate { get; set; }
        public object data { get; set; }
        public int seq { get; set; }
        public string msg { get; set; }
    }

    public class CRUDActor : ReceiveActor
    {
        internal IAccountService _accountService;

        public CRUDActor()
        {
            Receive<IAccountService>(m => {
                _accountService = m;
                Sender.Tell(new CRUDAction() { msg = "ok"});
            });

            Receive<CRUDAction>(m => {
                switch (m.action)
                {
                    case "insert":
                        _accountService.AddUser(m.data as User);
                        break;
                    case "delete":
                        _accountService.DelUser(m.data as User);
                        break;
                    default:
                        throw new Exception("unsport msg");
                       
                }
                if(m.isupdate) _accountService.UpdateDB();

                Sender.Tell(new CRUDAction() { action = m.action ,seq=m.seq,msg="ok"  });
            });
        }
    }
}
