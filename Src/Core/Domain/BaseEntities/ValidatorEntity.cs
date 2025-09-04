using Barin.Framework.Domain.ModelContracts;

namespace Barin.Framework.Domain.BaseEntities;

/// <summary>
/// Generic Validator Entity
/// </summary>
/// <typeparam name="TKey">Primary Key Type</typeparam>
public abstract class ValidatorEntity<TKey> : IModel<TKey>, IValidatorModel
{
    /// <summary>
    /// شناسه
    /// </summary>
    public virtual TKey Id { get; set; }

    /// <summary>
    /// Validator Entity
    /// </summary>
    public abstract void ValidateAndThrow();
}
