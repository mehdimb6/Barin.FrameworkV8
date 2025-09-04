namespace Barin.Framework.Domain.ModelContracts;

/// <summary>
/// Generic Validator Model Contract
/// </summary>
public interface IValidatorModel
{
    /// <summary>
    /// Validator Entity
    /// </summary>
    void ValidateAndThrow();
}
