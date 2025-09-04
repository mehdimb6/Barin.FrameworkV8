using Barin.Framework.Domain.ModelContracts;

namespace Barin.Framework.Domain.BaseEntities;

/// <summary>
/// Generic Base Entity
/// </summary>
/// <typeparam name="TKey">Primary Key Type</typeparam>
public class BaseEntity<TKey> : IBaseModel<TKey>
{
    /// <summary>
    /// شناسه
    /// </summary>
    public virtual TKey Id { get; set; }

    /// <summary>
    /// کاربر ایجاد کننده
    /// </summary>
    public virtual string? CreateBy { get; set; }

    /// <summary>
    /// زمان ایجاد
    /// </summary>
    public virtual DateTime? CreateDate { get; set; }
}
