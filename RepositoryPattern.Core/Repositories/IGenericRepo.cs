using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.Repositories
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T> CreateAsync(T item);
        T Update(T item);
        T Delete(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);

    }
}
