using accountapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Repository
{
    public interface ICurdRepo<T>
    {
        void AddUser(T user);
        void DelUser(T user);
        T GetUser(string userID);
        void UpdateDB();
    }
}
