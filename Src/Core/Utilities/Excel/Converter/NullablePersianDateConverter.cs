using Barin.Framework.Common.Helpers;

namespace Barin.Framework.Utilities.Excel.Converter;

public class NullablePersianDateConverter : BaseFieldConverter
{
    public override object Convert(string value)
    {
        try
        {
            return DateTimeHelper.ToMiladi(value);
        }
        catch
        {
            return null;
        }
    }
}
