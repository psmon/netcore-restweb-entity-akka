using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accountapi.Models;

namespace accountapi.Repository
{
    public interface ILocalEntity
    {
        String ID { get; set; }
    }

    public class LocalEntity<T> : ILocalEntity
    {
        public String ID { get; set; }
        public T Data { get; set; }
    }

    public class LocalRepository<T> : ICurdRepo<T>
    {
        List<T> localStorage = new List<T>();

        public void AddObj(T obj)
        {
            localStorage.Add(obj);
        }

        public void DelObj(T obj)
        {            
            localStorage.Remove(obj);
        }

        public T GetObj(T obj)
        {
            return localStorage.Find( t=>t.Equals(obj));
        }

        public T GetObj(string id)
        {
            T result = default(T);
            localStorage.ForEach(t =>
            {
                ILocalEntity data = t as ILocalEntity;
                if (data.ID == id) result = t;
            });
            return result;           
        }

        public void UpdateDB()
        {
            // -lazy update 미지원           
        }
    }
}
