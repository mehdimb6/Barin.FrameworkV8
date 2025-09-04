namespace Barin.Framework.Utilities.Excel.Converter;

public class DateTimeConverter : BaseFieldConverter
{
    public override object Convert(string value) => DateTime.TryParse(value, out DateTime result) ? result : default;
}