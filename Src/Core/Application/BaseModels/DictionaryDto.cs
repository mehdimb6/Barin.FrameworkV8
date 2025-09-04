namespace Barin.Framework.Application.BaseModels;

/// <summary>
/// مدل عمومی دیکشنری
/// </summary>
public class DictionaryDto : DictionaryDto<string, object>
{ 
}

/// <summary>
/// مدل عمومی دیکشنری
/// </summary>
public class DictionaryDto<TKey, TValue>
{
    /// <summary>
    /// کلید 
    /// </summary>
    public TKey? Key { get; set; }

    /// <summary>
    /// مقدار
    /// </summary>
    public TValue? Value { get; set; }
}
