using Barin.Framework.Application.Infrastructure;
using Barin.Framework.Domain.ModelContracts;
using System.Data;

namespace Barin.Framework.Repository.Dapper;

public class ReadOnlyGenericRepository<TEntity, TKey> : RepositoryBase, IReadOnlyGenericRepository<TEntity, TKey> where TEntity : class, IModel<TKey> where TKey : struct
{
    public ReadOnlyGenericRepository(IDbTransaction transaction) : base(transaction)
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
}
