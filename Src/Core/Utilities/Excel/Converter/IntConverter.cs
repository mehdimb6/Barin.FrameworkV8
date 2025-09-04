namespace Barin.Framework.Utilities.Excel.Converter;

public class IntConverter : BaseFieldConverter
{
    public override object Convert(string value) => int.TryParse(value, out int result) ? result : default;
}