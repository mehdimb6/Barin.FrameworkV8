using Barin.Framework.Common.Helpers;
using Barin.Framework.Utilities.Enums;
using Barin.Framework.Utilities.Excel.Attribute;
using Barin.Framework.Utilities.Excel.Converter;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using System.Reflection;
using LoadOptions = GemBox.Spreadsheet.LoadOptions;
using SaveOptions = GemBox.Spreadsheet.SaveOptions;

namespace Barin.Framework.Utilities.Excel;

internal class GemBoxExcelDataProvider : IExcelDataProvider
{
    public GemBoxExcelDataProvider()
    {
    }

    public GemBoxExcelDataProvider(IPropertyProvider propertyProvider)
    {
        PropertyProvider = propertyProvider;
    }

    public GemBoxExcelDataProvider(List<PropertyColumn> propertyColumns)
    {
        PropertyColumns = propertyColumns;
    }

    public IPropertyProvider PropertyProvider { get; set; }
    public List<PropertyColumn> PropertyColumns { get; set; }

    public IList<T> Read<T>(Stream stream, ExcelFormat format, string sheetName) where T : new()
    {
        // If using Professional version, put your serial key below.
        SpreadsheetInfo.SetLicense("EQU2-1000-0000-000U");
        ExcelFile excelFile; 

        switch (format)
        {
            case ExcelFormat.XLS:
                excelFile = ExcelFile.Load(stream, LoadOptions.XlsDefault);
                break;
            case ExcelFormat.XLSX:
                excelFile = ExcelFile.Load(stream, LoadOptions.XlsxDefault);
                break;
            case ExcelFormat.CSV:
                excelFile = ExcelFile.Load(stream, LoadOptions.CsvDefault);
                break;
            default:
                throw new ArgumentOutOfRangeException("format, value=" + format);
        }

        return ReadExcelFile<T>(sheetName, excelFile);
    }

    public IList<T> Read<T>(Stream stream, ExcelFormat format, string sheetName, List<IColumnOrder> columnOrders) where T : new()
    {
        // If using Professional version, put your serial key below.
        SpreadsheetInfo.SetLicense("EQU2-1000-0000-000U");
        ExcelFile excelFile;
        switch (format)
        {
            case ExcelFormat.XLS:
                excelFile = ExcelFile.Load(stream, LoadOptions.XlsDefault);
                break;
            case ExcelFormat.XLSX:
                excelFile = ExcelFile.Load(stream, LoadOptions.XlsxDefault);
                break;
            case ExcelFormat.CSV:
                excelFile = ExcelFile.Load(stream, LoadOptions.CsvDefault);
                break;
            default:
                throw new ArgumentOutOfRangeException("format, value=" + format);
        }

        return ReadExcelFile<T>(sheetName, excelFile, columnOrders);
    }

    public byte[] Write<T>(IList<T> list, string sheetName, ExcelFormat format, bool deleteHeader = false) where T : new()
    {
        // If using Professional version, put your serial key below.
        SpreadsheetInfo.SetLicense("EQU2-1000-0000-000U");
        //SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
        var excelFile = new ExcelFile();
        var workSheet = excelFile.Worksheets.Add(sheetName);
        workSheet.ViewOptions.ShowColumnsFromRightToLeft = true;

        if (list.Count == 0)
        {
            List<OrderedPropertyDescriptor> headers;
            try
            {
                var attBasedPropertyProvider = new AttributeBasedPropertyProvider();
                headers = attBasedPropertyProvider.GetProperties<T>().OrderBy(c => c.Order).ToList();
            }
            catch (Exception)
            {
                headers = null;
            }
            if (headers == null || headers.Count == 0)
                return new byte[0];
            AddHeaderRow<T>(headers, workSheet);
        }
        else
        {
            var properties = PropertyProvider.GetProperties(list.First()).OrderBy(c => c.Order).ToList();
            //t.GetType().GetProperties().Where(x => x.GetCustomAttribute<ExportableAttribute>()!=null)
            //.Select(x => new { x, Att = GetExportableInfo(x) }).OrderBy(x => x.Att.Order).ToArray();

            if (typeof(T) == typeof(Dictionary<string, object>))
            {
                for (var i = 0; i < properties.Count; i++)
                {
                    var prop = properties[i];
                    workSheet.Cells[0, i].Value = prop.Title;
                    if (prop.Width > 0)
                    {
                        workSheet.Columns[i].Width =
                            Convert.ToInt32(LengthUnitConverter.Convert(Convert.ToDouble(prop.Width),
                                LengthUnit.Pixel, LengthUnit.ZeroCharacterWidth256thPart));
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    var obj = list[i] as Dictionary<string, object>;
                    for (int j = 0; j < properties.Count; j++)
                    {
                        var prop = properties[j];
                        if (obj.ContainsKey(prop.Name))
                        {
                            workSheet.Cells[i + 1, j].Value = obj[prop.Name];
                            if (obj[prop.Name] != null)
                            {
                                var style = workSheet.Cells[i + 1, j].Style;
                                if (
                                    (new[]
                                    {
                                        typeof (decimal), typeof (long), typeof (decimal?), typeof (double?),
                                        typeof (double)
                                    }).Contains(obj[prop.Name].GetType()))
                                    style.NumberFormat = "#,#";
                                else if ((new[] { typeof(string) }).Contains(obj[prop.Name].GetType()))
                                    style.WrapText = style.IsTextVertical;
                            }
                        }
                    }
                }
            }
            else
            {
                AddHeaderRow<T>(properties, workSheet);

                // Add Content Rows To Worksheet 
                for (int i = 0; i < list.Count; i++)
                {
                    var obj = list[i];
                    for (int j = 0; j < properties.Count; j++)
                    {
                        var prop = properties[j];
                        workSheet.Cells[i + 1, j].Value = obj.GetPropertyValue(prop.PropertyInfo.Name);

                    }
                }
            }
        }

        if (deleteHeader)
            workSheet.Rows[0].Delete();

        using (var stream = new MemoryStream())
        {
            SaveOptions saveOption;
            switch (format)
            {
                case ExcelFormat.XLS:
                    saveOption = SaveOptions.XlsDefault;
                    break;
                case ExcelFormat.XLSX:
                    saveOption = SaveOptions.XlsxDefault;
                    break;
                case ExcelFormat.CSV:
                    saveOption = SaveOptions.CsvDefault;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("format, value=" + format);
            }

            excelFile.Save(stream, saveOption);
            return stream.ToArray();
        }
    }

    public IList<T> ReadWithCache<T>(string file, ExcelFormat format, string sheetName) where T : new()
    {
        // If using Professional version, put your serial key below.
        SpreadsheetInfo.SetLicense("EQU2-1000-0000-000U");
        var excelFile = LoadExcelFile(file, format);

        return ReadExcelFile<T>(sheetName, excelFile);
    }

    private static IList<T> ReadExcelFile<T>(string sheetName, ExcelFile excelFile) where T : new()
    {
        var workSheetInfo = GetWorkSheetInfo(typeof(T));
        ExcelWorksheet sheet;
        if (workSheetInfo != null && !string.IsNullOrWhiteSpace(workSheetInfo.SheetName))
            sheet =
                excelFile.Worksheets.FirstOrDefault(
                    x => string.Equals(x.Name, workSheetInfo.SheetName, StringComparison.CurrentCultureIgnoreCase));
        else
            sheet = excelFile.Worksheets.FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(sheetName))
            sheet =
                excelFile.Worksheets.FirstOrDefault(
                    x => string.Equals(x.Name, sheetName, StringComparison.CurrentCultureIgnoreCase));

        if (sheet == null)
            throw new Exception("Invalid Excel Sheet");
        var startRow = workSheetInfo != null ? workSheetInfo.StartRowNumber - 1 : 0;
        if (startRow <= 0)
            startRow = 0;

        var returnList = new List<T>();
        for (var i = startRow; i < sheet.Rows.Count; i++)
        {
            var row = sheet.Rows[i];

            if (!RowHasValue(row))
                continue;

            var dto = FillDto<T>(row);

            returnList.Add(dto);
        }

        return returnList;
    }

    private static IList<T> ReadExcelFile<T>(string sheetName, ExcelFile excelFile, List<IColumnOrder> columnOrders) where T : new()
    {
        var workSheetInfo = GetWorkSheetInfo(typeof(T));
        ExcelWorksheet sheet;
        if (workSheetInfo != null && !string.IsNullOrWhiteSpace(workSheetInfo.SheetName))
            sheet =
                excelFile.Worksheets.FirstOrDefault(
                    x => string.Equals(x.Name, workSheetInfo.SheetName, StringComparison.CurrentCultureIgnoreCase));
        else
            sheet = excelFile.Worksheets.FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(sheetName))
            sheet =
                excelFile.Worksheets.FirstOrDefault(
                    x => string.Equals(x.Name, sheetName, StringComparison.CurrentCultureIgnoreCase));

        if (sheet == null)
            throw new Exception("Invalid Excel Sheet");
        var startRow = workSheetInfo != null ? workSheetInfo.StartRowNumber - 1 : 0;
        if (startRow <= 0)
            startRow = 0;

        var returnList = new List<T>();
        for (var i = startRow; i < sheet.Rows.Count; i++)
        {
            var row = sheet.Rows[i];

            if (!RowHasValue(row))
                continue;

            var dto = FillDtoByOrderColumn<T>(row, columnOrders);

            returnList.Add(dto);
        }

        return returnList;
    }

    public static Dictionary<string, ExcelFile> CacheExcelFiles = new Dictionary<string, ExcelFile>();

    private static ExcelFile LoadExcelFile(string file, ExcelFormat format)
    {
        if (CacheExcelFiles.ContainsKey(file))
            return CacheExcelFiles[file];

        ExcelFile excelFile;
        switch (format)
        {
            case ExcelFormat.XLS:
                excelFile = ExcelFile.Load(file, LoadOptions.XlsDefault);
                break;
            case ExcelFormat.XLSX:
                excelFile = ExcelFile.Load(file, LoadOptions.XlsxDefault);
                break;
            case ExcelFormat.CSV:
                excelFile = ExcelFile.Load(file, LoadOptions.CsvDefault);
                break;
            default:
                throw new ArgumentOutOfRangeException("format, value=" + format);
        }

        CacheExcelFiles[file] = excelFile;

        return excelFile;
    }

    private static void AddHeaderRow<T>(List<OrderedPropertyDescriptor> properties, ExcelWorksheet workSheet) where T : new()
    {
        //Add Header Row to WorkSheet 
        for (var i = 0; i < properties.Count; i++)
        {
            var prop = properties[i];
            workSheet.Cells[0, i].Value = prop.Title;
        }
    }

    public byte[] WriteJArray(JArray jArray, string sheetName, ExcelFormat format)
    {
        // If using Professional version, put your serial key below.
        SpreadsheetInfo.SetLicense("EQU2-1000-0000-000U");
        var excelFile = new ExcelFile();
        var workSheet = excelFile.Worksheets.Add(sheetName);
        workSheet.ViewOptions.ShowColumnsFromRightToLeft = true;

        if (jArray.Any())
        {

            for (var k = 0; k < this.PropertyColumns.Count; k++)
            {
                workSheet.Cells[0, k].Value = PropertyColumns[k].Title;
                if (PropertyColumns[k].Width > 0)
                {
                    workSheet.Columns[k].Width = Convert.ToInt32(LengthUnitConverter.Convert(Convert.ToDouble(PropertyColumns[k].Width), LengthUnit.Pixel, LengthUnit.ZeroCharacterWidth256thPart));
                }
            }

            for (var i = 0; i < jArray.Count; i++)
            {
                var properties = ((JObject)jArray[i]).Properties();

                int z = 0;
                foreach (var property in properties)
                {
                    //write column values which are selected
                    if (PropertyColumns.Select(x => x.Name).Contains(property.Name))
                    {
                        workSheet.Cells[i + 1, z].Value = property.Value.ToObject<string>();
                        z++;
                    }

                }
            }
        }
        using (var stream = new MemoryStream())
        {
            SaveOptions saveOption;
            switch (format)
            {
                case ExcelFormat.XLS:
                    saveOption = SaveOptions.XlsDefault;
                    break;
                case ExcelFormat.XLSX:
                    saveOption = SaveOptions.XlsxDefault;
                    break;
                case ExcelFormat.CSV:
                    saveOption = SaveOptions.CsvDefault;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("format, value=" + format);
            }

            excelFile.Save(stream, saveOption);
            return stream.ToArray();
        }
    }

    private static bool RowHasValue(ExcelRow row)
    {
        return row.AllocatedCells.Any(x => x.Value != null);
    }

    private static T FillDto<T>(ExcelRow row) where T : new()
    {
        var t = new T();
        var properties = t.GetType().GetProperties();

        foreach (var property in properties)
        {
            var fieldOrder = GetFieldOrderInfo(property);
            if (fieldOrder == null)
                continue;

            var excelCell = GetMappedCell(row, fieldOrder);
            if (excelCell == null)
                continue;

            var cellValue = GetCellValue(excelCell, property);
            try
            {
                property.SetValue(t, cellValue);
            }
            catch (Exception)
            {
                //throw ex;
                //var exceptionMessage =
                //    string.Format("Type Mismatch. Excel Cell({0}) is {1}, but property({2}) is {3}",
                //        excelCell.Column.Name, excelCell.ValueType, property.Name, property.PropertyType);
                //throw new ArgumentException(exceptionMessage, ex);
            }
        }

        return t;
    }

    private static T FillDtoByOrderColumn<T>(ExcelRow row, List<IColumnOrder> list) where T : new()
    {
        var t = new T();
        var properties = t.GetType().GetProperties();

        foreach (var property in properties)
        {
            var fieldOrder = list.FirstOrDefault(x => x.PropertyName == property.Name);
            if (fieldOrder == null)
                continue;

            var excelCell = row.AllocatedCells[fieldOrder.PropertyOrder];
            if (excelCell == null)
                continue;

            var cellValue = GetCellValue(excelCell, property);
            try
            {
                property.SetValue(t, cellValue);
            }
            catch (Exception)
            {
                //throw ex;
                //var exceptionMessage =
                //    string.Format("Type Mismatch. Excel Cell({0}) is {1}, but property({2}) is {3}",
                //        excelCell.Column.Name, excelCell.ValueType, property.Name, property.PropertyType);
                //throw new ArgumentException(exceptionMessage, ex);
            }
        }

        return t;
    }

    private static ExcelCell GetMappedCell(ExcelRow row, FieldOrderAttribute fieldOrder)
    {
        var cellIndex = GetCellIndex(fieldOrder.Column);
        return row.AllocatedCells[cellIndex];
    }

    private static int GetCellIndex(string column)
    {
        var columns = new[]
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            "AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ",
            "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BZ",
            "CA","CB","CC","CD","CE","CF","CG","CH","CI","CJ","CK","CL","CM","CN","CO","CP","CQ","CR","CS","CT","CU","CV","CW","CX","CY","CZ",
            "DA","DB","DC","DD","DE","DF","DG","DH","DI","DJ","DK","DL","DM","DN","DO","DP","DQ","DR","DS","DT","DU","DV","DW","DX","DY","DZ",
            "EA","EB","EC","ED","EE","EF","EG","EH","EI","EJ","EK","EL","EM","EN","EO","EP","EQ","ER","ES","ET","EU","EV","EW","EX","EY","EZ",
            "FA","FB","FC","FD","FE","FF","FG","FH","FI","FJ","FK","FL","FM","FN","FO","FP","FQ","FR","FS","FT","FU","FV","FW","FX","FY","FZ",
            "GA","GB","GC","GD","GE","GF","GG","GH","GI","GJ","GK","GL","GM","GN","GO","GP","GQ","GR","GS","GT","GU","GV","GW","GX","GY","GZ"
        };

        for (var i = 0; i < columns.Length; i++)
        {
            if (String.Equals(column, columns[i], StringComparison.CurrentCultureIgnoreCase))
                return i;
        }

        throw new Exception("Invalid Column Name=" + column);
    }

    private static object GetCellValue(ExcelCell cell, MemberInfo propertyInfo)
    {
        var stringValue = cell.Value != null ? cell.Value.ToString() : "";

        BaseFieldConverter converter;
        return HasConverter(propertyInfo, out converter) ? converter.Convert(stringValue) : cell.Value;
    }

    private static bool HasConverter(MemberInfo type, out BaseFieldConverter converter)
    {
        converter = null;

        var attrs = System.Attribute.GetCustomAttributes(type);
        var converterAttr = attrs.OfType<FieldConverterAttribute>().FirstOrDefault();
        if (converterAttr == null)
            return false;

        converter = (BaseFieldConverter)Activator.CreateInstance(converterAttr.Type);
        return true;
    }

    private static WorkSheetAttribute GetWorkSheetInfo(MemberInfo type)
    {
        var attrs = System.Attribute.GetCustomAttributes(type);
        return attrs.OfType<WorkSheetAttribute>().FirstOrDefault();
    }

    private static FieldOrderAttribute GetFieldOrderInfo(MemberInfo type)
    {
        var attrs = System.Attribute.GetCustomAttributes(type);
        return attrs.OfType<FieldOrderAttribute>().FirstOrDefault();
    }
    private static ExportableAttribute GetExportableInfo(MemberInfo type)
    {
        var attrs = System.Attribute.GetCustomAttributes(type);
        return attrs.OfType<ExportableAttribute>().FirstOrDefault();
    }

}
