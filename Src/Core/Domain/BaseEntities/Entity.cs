using Barin.Framework.Domain.ModelContracts;

namespace Barin.Framework.Domain.BaseEntities;

/// <summary>
/// Generic Id Entity
/// </summary>
/// <typeparam name="TKey"></typeparam>
public class Entity<TKey> : IModel<TKey>
{
    /// <summary>
    /// شناسه
    /// </summary>
    public virtual TKey Id { get; set; }
}
