namespace Barin.Framework.Domain.ModelContracts;

/// <summary>
/// Generic Data Model Contract
/// </summary>
/// <typeparam name="TKey">Primary Key Type</typeparam>
public interface IDataModel<TKey> : IBaseModel<TKey>, IValidatorModel
{
}
