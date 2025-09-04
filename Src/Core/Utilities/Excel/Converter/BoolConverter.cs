namespace Barin.Framework.Utilities.Excel.Converter;

public class BoolConverter : BaseFieldConverter
{
    public override object Convert(string value)
    {
        var strValue = value.Trim().ToLower();
        switch (strValue)
        {
            case "1": return true;
            case "0": return false;
        }

        return bool.TryParse(value, out bool result) ? result : default;
    }
}