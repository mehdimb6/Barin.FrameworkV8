namespace Barin.Framework.Application.BaseModels;

/// <summary>
/// صفحه بندی یکپارچه تمامی نتایج چند صفحه ای
/// </summary>
public class PagedList<T> : PageItem
{
    public PagedList()
    {
        TotalCount = -1;
        Items = new List<T>();
    }

    public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    /// <summary>
    /// لیست نتایج
    /// </summary>
    public IEnumerable<T> Items { get; set; }

    /// <summary>
    /// تعداد کل نتایج
    /// </summary>
    public int TotalCount { get; set; }
}
