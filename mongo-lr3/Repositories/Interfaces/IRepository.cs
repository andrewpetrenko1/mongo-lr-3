using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace mongo_lr3.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Insert(TEntity entity);
        Task<IEnumerable<TEntity>> InsertList(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetList(FilterDefinition<TEntity> filter);
        Task<TEntity> Get(Expression<Func<TEntity, object>> field, object value);
        Task<long> Update(Expression<Func<TEntity, object>> field, object value, TEntity entity);
        Task<long> Delete(Expression<Func<TEntity, object>> field, object value);
        Task<long> DeleteAll();
        bool IsEmptyCollection();
    }
}
