﻿using accountapi.Controllers;
using accountapi.Models;
using accountapi.Repository;
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
        internal ICurdRepo<User> _repository;

        public CRUDActor()
        {
            // CRUD를 실시간 교체하여 RDB<->LOCALDB 전환이 가능
            Receive<ICurdRepo<User>>(m => {
                _repository = m;
                Sender.Tell(new CRUDAction() { msg = "ok"});
            });

            Receive<CRUDAction>(m => {
                switch (m.action)
                {
                    case "insert":
                        _repository.AddObj(m.data as User);
                        break;
                    case "delete":
                        _repository.DelObj(m.data as User);
                        break;
                    default:
                        throw new Exception("unsport msg");
                       
                }
                if(m.isupdate) _repository.UpdateDB();

                Sender.Tell(new CRUDAction() { action = m.action ,seq=m.seq,msg="ok"  });
            });
        }
    }
}
