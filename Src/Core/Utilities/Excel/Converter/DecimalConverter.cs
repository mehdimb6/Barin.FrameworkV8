namespace Barin.Framework.Utilities.Excel.Converter;

public class DecimalConverter : BaseFieldConverter
{
    public override object Convert(string value) => decimal.TryParse(value, out decimal result) ? result : default;
}