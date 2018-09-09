using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accountapi.Models;

namespace accountapi.Repository
{

    public class LocalRepository : ICurdRepo<User>
    {
        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DelUser(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string userID)
        {
            throw new NotImplementedException();
        }

        public void UpdateDB()
        {
            throw new NotImplementedException();
        }
    }
}
