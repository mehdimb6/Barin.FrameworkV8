using Barin.Framework.Utilities.Enums;
using ClosedXML.Excel;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Barin.Framework.Utilities.Excel;

public static class ExcelHelper
{
    private static IExcelDataProvider ExcelDataProvider
    {
        get
        {
            return new GemBoxExcelDataProvider(new AttributeBasedPropertyProvider());
        }
    }

    public static IList<T> ReadWithCache<T>(string filePath, ExcelFormat format, string sheetName = "") where T : new()
    {
        return ExcelDataProvider.ReadWithCache<T>(filePath, format, sheetName);
    }

    public static IList<T> Read<T>(Stream stream, ExcelFormat format, string sheetName = "") where T : new()
    {
        return ExcelDataProvider.Read<T>(stream, format, sheetName);
    }

    public static IList<T> Read<T>(Stream stream, ExcelFormat format, string sheetName, List<IColumnOrder> columnOrders) where T : new()
    {
        return ExcelDataProvider.Read<T>(stream, format, sheetName, columnOrders);
    }

    public static byte[] Write<T>(IList<T> list, ExcelFormat format, string sheetName = "") where T : new()
    {
        return ExcelDataProvider.Write(list, sheetName, format);
    }

    public static byte[] Write<T>(IList<T> list, ExcelFormat format, bool deleteHeader = false, string sheetName = "") where T : new()
    {
        return ExcelDataProvider.Write(list, sheetName, format, deleteHeader);
    }

    public static byte[] Write<T>(IList<T> list, string sheetName = "") where T : new()
    {
        return ExcelDataProvider.Write(list, sheetName, ExcelFormat.XLSX);
    }

    public static byte[] Write<T>(IList<T> list, ExcelFormat format, IPropertyProvider propertyProvider, string sheetName = "") where T : new()
    {
        var provider = new GemBoxExcelDataProvider(propertyProvider);
        return provider.Write(list, sheetName, format);
    }

    public static byte[] WriteJArray(JArray list, ExcelFormat format, List<PropertyColumn> propertyColumns, string sheetName = "")
    {
        var provider = new GemBoxExcelDataProvider(propertyColumns);
        return provider.WriteJArray(list, sheetName, format);
    }

    /// <summary>
    /// تبدیل اطلاعات به اکسل
    /// </summary>
    /// <param name="dataTable">منبع اطلاعات</param>
    /// <param name="exportPath">مسیر ذخیره سازی به همراه نام فایل بدون پسوند</param>
    public static void CreateExcelPure(DataTable dataTable, string exportPath)
    {
        CreateExcelPure(dataTable, "Sheet1", exportPath);
    }

    /// <summary>
    /// تبدیل اطلاعات به اکسل
    /// </summary>
    /// <param name="dataTable">منبع اطلاعات</param>
    /// <param name="sheetName">نام شیت</param>
    /// <param name="exportPath">مسیر ذخیره سازی به همراه نام فایل بدون پسوند</param>
    public static void CreateExcelPure(DataTable dataTable, string sheetName, string exportPath)
    {
        var wb = new XLWorkbook();
        wb.Worksheets.Add(dataTable, sheetName);
        wb.SaveAs($"{exportPath}.xlsx");
    }

    /// <summary>
    /// تبدیل اطلاعات به اکسل
    /// </summary>
    /// <param name="dataTables">لیست منبع اطلاعات</param>
    /// <param name="exportPath">مسیر ذخیره سازی به همراه نام فایل بدون پسوند</param>
    public static void CreateExcelPure(List<DataTable> dataTables, string exportPath)
    {
        var wb = new XLWorkbook();
        var i = 1;

        foreach (var dataTable in dataTables)
            wb.Worksheets.Add(dataTable, $"Sheet{i++}");

        wb.SaveAs($"{exportPath}.xlsx");
    }

    /// <summary>
    /// تبدیل اطلاعات به اکسل
    /// </summary>
    /// <param name="dataTables">دیکشنری نام شیت به همراه منبع اطلاعات</param>
    /// <param name="exportPath">مسیر ذخیره سازی به همراه نام فایل بدون پسوند</param>
    public static void CreateExcelPure(Dictionary<string, DataTable> dataTables, string exportPath)
    {
        var wb = new XLWorkbook();

        foreach (var dataTable in dataTables)
            wb.Worksheets.Add(dataTable.Value, dataTable.Key);

        wb.SaveAs($"{exportPath}.xlsx");
    }
}
