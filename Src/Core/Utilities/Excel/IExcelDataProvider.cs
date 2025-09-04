using Barin.Framework.Utilities.Enums;

namespace Barin.Framework.Utilities.Excel;

internal interface IExcelDataProvider
{
    IPropertyProvider PropertyProvider { get; set; }
    IList<T> Read<T>(Stream stream, ExcelFormat format, string sheetName) where T : new();
    IList<T> Read<T>(Stream stream, ExcelFormat format, string sheetName, List<IColumnOrder> columnOrders) where T : new();
    byte[] Write<T>(IList<T> list, string sheetName, ExcelFormat format, bool deleteHeader = false) where T : new();
    IList<T> ReadWithCache<T>(string file, ExcelFormat format, string sheetName) where T : new();
}
