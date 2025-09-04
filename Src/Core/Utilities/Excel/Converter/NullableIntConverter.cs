namespace Barin.Framework.Utilities.Excel.Converter;

public class NullableIntConverter : BaseFieldConverter
{
    public override object Convert(string value) => int.TryParse(value, out int result) ? (int?)result : null;
}