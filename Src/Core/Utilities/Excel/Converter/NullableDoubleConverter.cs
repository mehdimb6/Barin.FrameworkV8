namespace Barin.Framework.Utilities.Excel.Converter;

public class NullableDoubleConverter : BaseFieldConverter
{
    public override object Convert(string value) => double.TryParse(value, out double result) ? (double?)result : null;
}