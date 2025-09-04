namespace Barin.Framework.Common.Helpers;

public static class FluentValidationHelper
{
    /// <summary>
    /// متن خطا را از فرمت پیش فرض آن جدا می کند
    /// </summary>        
    public static string Message(string exceptionMessage)
    {
        string message = exceptionMessage;
        int length = exceptionMessage.Length;
        int position = exceptionMessage.IndexOf("--");

        return message.Substring(position + 2, length - position - 2);
    }
}
