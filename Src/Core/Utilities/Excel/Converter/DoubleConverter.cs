namespace Barin.Framework.Utilities.Excel.Converter;

public class DoubleConverter : BaseFieldConverter
{
    public override object Convert(string value) => double.TryParse(value, out double result) ? result : default;
}
