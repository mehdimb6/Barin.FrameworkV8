namespace Barin.Framework.Domain.ModelContracts;

/// <summary>
/// Generic Audit Model Contract
/// </summary>
public interface IAuditModel
{
    string CreateBy { get; set; }
    DateTime? CreateDate { get; set; }
}
