namespace Barin.Framework.Domain.ModelContracts;

/// <summary>
/// Generic Model Contract
/// </summary>
/// <typeparam name="TKey">Primary Key Type</typeparam>
public interface IModel<TKey>
{
    /// <summary>
    /// Primary Key
    /// </summary>
    TKey Id { get; set; }
}
