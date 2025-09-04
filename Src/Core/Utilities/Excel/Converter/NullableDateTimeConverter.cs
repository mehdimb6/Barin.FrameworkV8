namespace Barin.Framework.Utilities.Excel.Converter;

public class NullableDateTimeConverter : BaseFieldConverter
{
    public override object Convert(string value) => DateTime.TryParse(value, out DateTime result) ? (DateTime?)result : null;
}
