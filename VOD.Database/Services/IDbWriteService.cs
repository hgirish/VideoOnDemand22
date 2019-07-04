using System.Threading.Tasks;

namespace VOD.Database.Services
{
    public interface IDbWriteService
    {
        Task<bool>  SaveChangesAsync();
        void Add<TEntiry>(TEntiry item) where TEntiry : class;
        void Delete<TEntity>(TEntity item) where TEntity : class;
        void Update<TEntity>(TEntity item) where TEntity : class;
    }
}
