namespace Barin.Framework.Application.BaseModels;

/// <summary>
/// مدل مشترک لوک آپ
/// </summary>
public class LookupDto<T>
{
    /// <summary>
    /// شناسه 
    /// </summary>
    public T? Id { get; set; }

    /// <summary>
    /// شرح
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// مدل مشترک لوک آپ
/// </summary>
public class LookupDto : LookupDto<int>
{
}
