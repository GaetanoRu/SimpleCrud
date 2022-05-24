using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCrud.DataAccessLayer
{
    public interface IDataContext
    {
        IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : class;
        public ValueTask<T> GetAsync<T>(params object[] keyValues) where T : class;
        void Insert<T>(T entity) where T : class;
        void Edit<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task SaveAsync();
    }
}
