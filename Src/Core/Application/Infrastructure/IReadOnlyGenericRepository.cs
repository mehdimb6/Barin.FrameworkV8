using Barin.Framework.Domain.ModelContracts;

namespace Barin.Framework.Application.Infrastructure;

/// <summary>
/// ریپازیتوری رابطی جهت بازیابی اطلاعات از بانک اطلاعاتی است 
/// که پیچیدگی های اتصال و کار با بانک اطلاعاتی در آن قرار دارد
/// </summary>
public interface IReadOnlyGenericRepository<TEntity, TOut> where TEntity : IModel<TOut>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(TOut id);
}
