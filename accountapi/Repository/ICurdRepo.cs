using accountapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Repository
{
    public interface ICurdRepo<T>
    {
        void AddObj(T obj);
        void DelObj(T obj);
        T GetObj(string id);
        void UpdateDB();
    }
}
