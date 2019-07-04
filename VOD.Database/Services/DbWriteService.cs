﻿using System.Threading.Tasks;
using VOD.Database.Contexts;

namespace VOD.Database.Services
{
    public class DbWriteService : IDbWriteService
    {
        
        private readonly VODContext _db;
        
      
        public DbWriteService(VODContext db)
        {
            _db = db;
        }

        public void Add<TEntiry>(TEntiry item) where TEntiry : class
        {
            try
            {
                _db.Add(item);
            }
            catch
            {
                throw;
            }
        }

        public void Delete<TEntity>(TEntity item) where TEntity : class
        {
            try
            {
                _db.Set<TEntity>().Remove(item);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _db.SaveChangesAsync() >= 0;
            }
            catch 
            {
                return false;
            }
        }

        public void Update<TEntity>(TEntity item) where TEntity : class
        {
            try
            {
                var entity = _db.Find<TEntity>(item.GetType()
                    .GetProperty("Id").GetValue(item));
                if (entity != null)
                {
                    _db.Entry(entity).State =
                        Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
                _db.Set<TEntity>().Update(item);
            } 
            catch
            {
                throw;
            }
        }
    }
}
