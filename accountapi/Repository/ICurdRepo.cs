using accountapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Repository
{
    public interface ICurdRepo
    {
        void AddUser(User user);
        void DelUser(User user);
        User GetUser(string userID);
        void UpdateDB();
    }
}
