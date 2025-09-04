namespace Barin.Framework.Application.BaseModels;

/// <summary>
/// صفحه بندی یکپارچه تمامی نتایج چند صفحه ای اضافه
/// </summary>
public class PagedListEx<T> : PagedList<T>
{
    public PagedListEx()
    {
        ExItems = new List<DictionaryDto>();
    }

    public PagedListEx(IEnumerable<T> items, IEnumerable<DictionaryDto> exItems, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        ExItems = exItems;
    }

    /// <summary>
    /// پارامترهای اضافه
    /// </summary>
    public IEnumerable<DictionaryDto> ExItems { get; set; }
}

