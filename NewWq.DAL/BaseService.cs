using NewWq.IDAL;
using NewWq.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.DAL
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity, new()
    {
        protected readonly WebContext _db;
        public BaseService(WebContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(T model, bool saved = true)
        {
            _db.Set<T>().Add(model);
            if (saved)
            {
                await _db.SaveChangesAsync();
            }
        }
        public async Task EditAsync(T model, bool saved = true)
        {
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            if (saved)
            {
                //SaveChanges前先关闭验证实体有效性（ValidateOnSaveEnabled）这个开关
                _db.Configuration.ValidateOnSaveEnabled = false;
                await _db.SaveChangesAsync();
                _db.Configuration.ValidateOnSaveEnabled = true;
            }
        }


        public async Task RemoveAsync(Guid id, bool saved = true)
        {
            var t = new T() { Id = id };
            _db.Entry(t).State = System.Data.Entity.EntityState.Unchanged;
            t.IsRemoved = true;
            if (saved)
            {
                _db.Configuration.ValidateOnSaveEnabled = false;
                await _db.SaveChangesAsync();
                _db.Configuration.ValidateOnSaveEnabled = true;
            }
        }

        public async Task RemoveAsync(T model, bool saved = true)
        {
            await RemoveAsync(model.Id, saved);
        }

        public async Task SaveAsync()
        {
            _db.Configuration.ValidateOnSaveEnabled = false;
            await _db.SaveChangesAsync();
            _db.Configuration.ValidateOnSaveEnabled = true;
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<T> GetOneByIdAsync(Guid id)
        {
            return await GetAll().FirstAsync(m => m.Id == id);
        }

        public IQueryable<T> GetAll()
        {
            return _db.Set<T>().AsNoTracking().Where(m => !m.IsRemoved);
        }
        public IQueryable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }
        public IQueryable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate, bool asc = true)
        {
            var data = GetAll(predicate);
            if (asc)
            {
                return data.OrderBy(m => m.CreateTime);
            }
            return data.OrderByDescending(m => m.CreateTime);
        }
        public IQueryable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate, bool asc = true, int pageSize = 10, int pageIndex = 1)
        {
            return GetAll(predicate, asc).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        }


    }
}
