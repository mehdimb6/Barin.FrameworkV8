using Barin.Framework.Domain.ModelContracts;

namespace Barin.Framework.Domain.BaseEntities;

/// <summary>
/// Generic Data Entity
/// </summary>
/// <typeparam name="TKey">Primary Key Type</typeparam>
public abstract class DataEntity<TKey> : IDataModel<TKey>
{
    /// <summary>
    /// شناسه
    /// </summary>
    public virtual TKey Id { get; set; }

    /// <summary>
    /// کاربر ایجاد کننده
    /// </summary>
    public virtual string CreateBy { get; set; }

    /// <summary>
    /// زمان ایجاد
    /// </summary>
    public virtual DateTime? CreateDate { get; set; }

    /// <summary>
    /// Validator Entity
    /// </summary>
    public abstract void ValidateAndThrow();
}
