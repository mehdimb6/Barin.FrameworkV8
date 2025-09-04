using Barin.Framework.Application.Enums;

namespace Barin.Framework.Application.BaseModels;

/// <summary>
/// صفحه بندی یکپارچه تمامی درخواست ها صفحه بندی شده
/// مرتب شده
/// </summary>
public class PageSortItem : PageItem
{
    /// <summary>
    /// عبارت مرتب سازی
    /// </summary>
    public string? SortExpression { get; set; }

    /// <summary>
    /// عبارت مرتب سازی
    /// </summary>
    public SortDirectionType SortDirection { get; set; } = SortDirectionType.Asc;

    /// <summary>
    /// دریافت عبارت مرتب سازی معتبر براساس فیلدهای موجود در موجودیت
    /// </summary>
    public string GetSortString<TEntity>(string? expression, SortDirectionType direction, string? aliasMainTable = null)
    {
        if (aliasMainTable != null)
            aliasMainTable = $"{aliasMainTable}.";

        var sortDirection = "";
        if (direction == SortDirectionType.Desc)
            sortDirection = "desc";

        if (string.IsNullOrWhiteSpace(expression))
            return $" ORDER BY {aliasMainTable}Id {sortDirection}";

        expression = expression.ToLower().Trim();

        var objType = typeof(TEntity);
        var properties = objType.GetProperties();

        foreach (var property in properties)
        {
            if (property.Name.ToLower() == expression)
                return $" ORDER BY {aliasMainTable}{expression} {sortDirection}";
        }

        return $" ORDER BY {aliasMainTable}Id {sortDirection}";
    }
}
