using Barin.Framework.Domain.ModelContracts;

namespace Barin.Framework.Domain.BaseEntities;

/// <summary>
/// Generic Audit Entity
/// </summary>
public class AuditEntity : IAuditModel
{
    /// <summary>
    /// کاربر ایجاد کننده
    /// </summary>
    public virtual string? CreateBy { get; set; }

    /// <summary>
    /// زمان ایجاد
    /// </summary>
    public virtual DateTime? CreateDate { get; set; }
}
