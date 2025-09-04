namespace Barin.Framework.Application.BaseModels;

/// <summary>
/// صفحه بندی یکپارچه تمامی درخواست ها صفحه بندی شده
/// </summary>
public class PageItem
{
    public PageItem()
    {
        PageNumber = -1;
        PageSize = -1;
    }

    /// <summary>
    /// شماره صفحه
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// تعداد آیتم در صفحه
    /// </summary>
    public int PageSize { get; set; }
}