using mongo_lr3.Models.Interfaces;
using mongo_lr3.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace mongo_lr3.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoCollection<TEntity> collection;
        public Repository(IMongoDatabase database)
        {
            collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> InsertList(IEnumerable<TEntity> entities)
        {
            await collection.InsertManyAsync(entities);
            return entities;
        }

        public async Task<IEnumerable<TEntity>> GetAll() => await collection.Find(_ => true).ToListAsync();

        public async Task<IEnumerable<TEntity>> GetList(FilterDefinition<TEntity> filter) => await collection.Find(filter).ToListAsync();

        public async Task<TEntity> Get(Expression<Func<TEntity, object>> field, object value)
        {
            var filter = Builders<TEntity>.Filter.Eq(field, value);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<long> Update(Expression<Func<TEntity, object>> field, object value, TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(field, value);
            return (await collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true})).ModifiedCount;
        }

        public async Task<long> Delete(Expression<Func<TEntity, object>> field, object value)
        {
            var filter = Builders<TEntity>.Filter.Eq(field, value);
            return (await collection.DeleteOneAsync(filter)).DeletedCount;
        }

        public async Task<long> DeleteAll()
        {
            return (await collection.DeleteManyAsync(x => x != null)).DeletedCount;
        }

        public bool IsEmptyCollection()
        {
            return collection.EstimatedDocumentCount() == 0;
        }
    }
}
