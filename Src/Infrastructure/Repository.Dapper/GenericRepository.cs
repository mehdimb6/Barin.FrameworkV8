using Barin.Framework.Application.Infrastructure;
using Barin.Framework.Domain.ModelContracts;
using System.Data;

namespace Barin.Framework.Repository.Dapper;

public abstract class GenericRepository<TEntity, TKey> : RepositoryBase, IGenericRepository<TEntity, TKey> where TEntity : class, IModel<TKey> where TKey : struct
{
    protected GenericRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Connection.GetAllAsync<TEntity>(Transaction);
    }

    public async Task<TEntity> GetByIdAsync(TKey id)
    {
        return await Connection.GetAsync<TEntity>(id, Transaction);
    }

    public async Task<TKey> InsertAsync(TEntity entity)
    {
        await Connection.InsertAsync(entity, Transaction);
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        return await Connection.UpdateAsync(entity, Transaction);
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        return await Connection.DeleteAsync(entity, Transaction);
    }

    public async Task<bool> DeleteAsync(TKey entityId)
    {
        var entity = await GetByIdAsync(entityId);
        if (entity != null)
            return await Connection.DeleteAsync(entity, Transaction);

        return false;
    }

    public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            await Connection.InsertAsync(entity, Transaction);
        }
    }
}
