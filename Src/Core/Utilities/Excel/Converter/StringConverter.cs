namespace Barin.Framework.Utilities.Excel.Converter;

public class StringConverter : BaseFieldConverter
{
    public override object Convert(string value)
    {
        try
        {
            return value;
        }
        catch
        {
            return string.Empty;
        }
    }
}