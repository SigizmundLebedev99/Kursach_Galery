using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.DAL.Repository
{
    public interface IRepository<T>
    {
        Task<T> CreateAsync(T entity);
        Task<T> FindByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
