namespace Barin.Framework.Utilities.Excel.Converter;

public class NullableDecimalConverter : BaseFieldConverter
{
    public override object Convert(string value) => decimal.TryParse(value, out decimal result) ? (decimal?)result : null;
}