using Barin.Framework.Common.Helpers;

namespace Barin.Framework.Utilities.Excel.Converter;

public class PersianDateConverter : BaseFieldConverter
{
    public override object Convert(string value)
    {
        try
        {
            return DateTimeHelper.ToMiladi(value);
        }
        catch
        {
            return default(DateTime);
        }
    }
}
