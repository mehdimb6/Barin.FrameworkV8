namespace Barin.Framework.Utilities.Excel.Converter;

public class NullableFloatConverter : BaseFieldConverter
{
    public override object Convert(string value) => float.TryParse(value, out float result) ? (float?)result : null;
}
