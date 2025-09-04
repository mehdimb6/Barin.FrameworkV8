namespace Barin.Framework.Domain.ModelContracts;

/// <summary>
/// Generic Base Model Contract
/// </summary>
/// <typeparam name="TKey">Primary Key Type</typeparam>
public interface IBaseModel<TKey> : IModel<TKey>, IAuditModel
{
}
