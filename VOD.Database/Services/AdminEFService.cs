using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VOD.Common.Services;

namespace VOD.Database.Services
{
    public class AdminEFService : IAdminService
    {
        private readonly IDbReadService _dbRead;
        private readonly IDbWriteService _dbWrite;
        private readonly IMapper _mapper;

        public AdminEFService(IDbReadService dbReadService,
            IDbWriteService dbWrite,
            IMapper mapper)
        {
            _dbRead = dbReadService;
            _dbWrite = dbWrite;
            _mapper = mapper;
        }
        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return await _dbRead.AnyAsync(expression);
        }

        public async Task<int> CreateAsync<TSource, TDestination>(TSource item)
            where TSource : class
            where TDestination : class
        {
            try
            {
                var entity = _mapper.Map<TDestination>(item);
                _dbWrite.Add(entity);
                var succeeded = await _dbWrite.SaveChangesAsync();
                if (succeeded)
                {
                    return (int)entity.GetType().GetProperty("Id").GetValue(entity);
                }
            }
            catch
            {

            }
            return -1;
        }

        public async  Task<bool> DeleteAsync<TSource>(Expression<Func<TSource, bool>> expression) where TSource : class
        {

            try
            {
                var entity = await _dbRead.SingleAsync<TSource>(expression);
                _dbWrite.Delete(entity);
                return await _dbWrite.SaveChangesAsync();
            }
            catch 
            {
                return false;
            }
        }

        public async  Task<List<TDestination>> GetAsync<TSource, TDestination>(bool include = false)
            where TSource : class
            where TDestination : class
        {
            if (include)
            {
                _dbRead.Include<TSource>();
            }
            var entities = await _dbRead.GetAsync<TSource>();
            return _mapper.Map<List<TDestination>>(entities);
        }

        public async  Task<List<TDestination>> GetAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false)
            where TSource : class
            where TDestination : class
        {
            if (include)
            {
                _dbRead.Include<TSource>();
            }
            var entities = await _dbRead.GetAsync<TSource>(expression);
            return _mapper.Map<List<TDestination>>(entities);
        }

        public async  Task<TDestination> SingleAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false)
            where TSource : class
            where TDestination : class
        {
            if (include)
            {
                _dbRead.Include<TSource>();
            }
            var entity = await _dbRead.SingleAsync<TSource>(expression);
            return _mapper.Map<TDestination>(entity);
        }

        public async  Task<bool> UpdateAsync<TSource, TDestination>(TSource item)
            where TSource : class
            where TDestination : class
        {
            try
            {
                var entity = _mapper.Map<TDestination>(item);
                _dbWrite.Update(entity);
                return await _dbWrite.SaveChangesAsync();
               
            }
            catch
            {

            }
            return false;
        }
    }
}
