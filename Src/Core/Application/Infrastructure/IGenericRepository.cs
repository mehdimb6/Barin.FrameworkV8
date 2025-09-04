using Barin.Framework.Domain.ModelContracts;

namespace Barin.Framework.Application.Infrastructure;

/// <summary>
/// بطور منطقی محل ذخیره سازی اطلاعات است ولی در حقیقت
/// ریپازیتوری رابطی جهت ذخیره و بازیابی اطلاعات با بانک اطلاعاتی است 
/// که پیچیدگی های اتصال و کار با بانک اطلاعاتی در آن قرار دارد
/// </summary>
public interface IGenericRepository<TEntity, TOut> where TEntity : IModel<TOut>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(TOut id);
    Task<TOut> InsertAsync(TEntity entity);
    Task InsertRangeAsync(IEnumerable<TEntity> entities);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<bool> DeleteAsync(TOut entityId);
}
