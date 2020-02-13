using NewWq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.IDAL
{
    public interface IBaseService<T> : IDisposable where T : BaseEntity
    {
        Task CreateAsync(T model, bool saved = true);
        Task EditAsync(T model, bool saved = true);
        Task RemoveAsync(Guid id, bool saved = true);
        Task RemoveAsync(T model, bool saved = true);
        Task SaveAsync();
        Task<T> GetOneByIdAsync(Guid id);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, bool asc = true);
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, bool asc, int pageSize = 10, int pageIndex = 1);
    }
}
