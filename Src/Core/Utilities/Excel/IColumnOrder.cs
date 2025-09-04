namespace Barin.Framework.Utilities.Excel;

public interface IColumnOrder
{
    string PropertyName { get; set; }
    string PropertyTitle { get; set; }
    int PropertyOrder { get; set; }
}
