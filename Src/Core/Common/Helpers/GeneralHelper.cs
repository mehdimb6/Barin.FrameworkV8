using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Barin.Framework.Common.Helpers;

public static class GeneralHelper
{
    /// <summary>
    /// بررسی وجود کاراکتر یونیکد در متن
    /// </summary>
    public static bool IsUnicode(this string str)
    {
        foreach (char ch in str)
            if ((int)ch > 255)
                return true;

        return false;
    }

    public static string Join(this List<string> value, string seperator)
    {
        return string.Join(seperator, value);
    }

    public static bool IsNullOrEmpty(this string text)
    {
        return string.IsNullOrEmpty(text);
    }

    public static bool IsNullOrWhiteSpace(this string text)
    {
        return string.IsNullOrWhiteSpace(text);
    }

    public static T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static T ToEnum<T>(this string value, T defaultValue)
    {
        if (value.IsNullOrWhiteSpace())
            return defaultValue;

        try
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        catch
        {
            return defaultValue;
        }
    }

    public static char Lock() => Convert.ToChar(0);

    public static char NumberLock(this char chr)
    {
        if (chr >= Convert.ToChar(48) && chr <= Convert.ToChar(57))
            chr = Convert.ToChar(0);

        return chr;
    }

    public static char TextLock(this char chr)
    {
        if (chr != Convert.ToChar(8) && (chr < Convert.ToChar(48) || chr > Convert.ToChar(57)))
            chr = Convert.ToChar(0);

        return chr;
    }

    public static char OnlyEnglishCharAndNumber(this char chr)
    {
        if (chr != Convert.ToChar(8) && (chr < Convert.ToChar(48) || chr > Convert.ToChar(57)) && (chr < Convert.ToChar(65) || chr > Convert.ToChar(90)) && (chr < Convert.ToChar(97) || chr > Convert.ToChar(122)))
            chr = Convert.ToChar(0);

        return chr;
    }

    public static string GetAppSettings(this string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException("key", "پارامتر ورودی نباید خالی باشد");

        return ConfigurationManager.AppSettings[key];
    }

    /// <summary>
    /// تعیین معتبر بودن کد ملی
    /// </summary>
    /// <param name="nationalCode">کد ملی وارد شده</param>
    /// <returns>
    /// در صورتی که کد ملی صحیح باشد خروجی <c>true</c> و در صورتی که کد ملی اشتباه باشد خروجی <c>false</c> خواهد بود
    /// </returns>
    public static string IsValidNationalCode(this string nationalCode)
    {
        //در صورتی که کد ملی وارد شده تهی باشد
        if (string.IsNullOrEmpty(nationalCode))
            return "لطفا کد ملی را صحیح وارد نمایید";

        //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
        if (nationalCode.Length != 10)
            return "طول کد ملی باید ده کاراکتر باشد";

        //در صورتی که کد ملی ده رقم عددی نباشد
        var regex = new Regex(@"\d{10}");
        if (!regex.IsMatch(nationalCode))
            return "کد ملی تشکیل شده از ده رقم عددی می‌باشد؛ لطفا کد ملی را صحیح وارد نمایید";

        //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
        var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
        if (allDigitEqual.Contains(nationalCode))
            return "کد ملی نامعتبر است";

        //عملیات شرح داده شده در بالا
        var chArray = nationalCode.ToCharArray();
        var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
        var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
        var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
        var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
        var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
        var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
        var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
        var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
        var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
        var a = Convert.ToInt32(chArray[9].ToString());

        var b = num0 + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
        var c = b % 11;

        var result = ((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a));

        return result ? "" : "کد ملی نامعتبر است";
    }

    /// <summary>
    /// بررسی فرمت نام فایل که تصویر باشد
    /// jpeg, jpg, png, bmp, gif, tif, tiff
    /// </summary>
    public static bool IsImageExtension(this string fileName)
    {
        var exts = new string[] { ".jpeg", ".jpg", ".png", ".bmp", ".gif", ".tif", ".tiff" };
        string ext = Path.GetExtension(fileName).ToLower();
        return Array.IndexOf(exts, ext) != -1;
    }

    public static string MoneyFormat(this string money)
    {
        if (money.IsNullOrWhiteSpace())
            return string.Empty;

        return decimal.Parse(money).ToString("N0");
    }

    public static string MoneyNoFormat(this string money)
    {
        return money.Replace(",", "");
    }

    public static bool CheckAmount(this string amount)
    {
        if (string.IsNullOrWhiteSpace(amount))
            return true;

        var flag = long.TryParse(MoneyNoFormat(amount), out long result);
        return flag;
    }

    public static int[] SplitTime(this string time)
    {
        if (time.Length == 5)
            time = TimeNoFormat(time);

        if (time.Length == 4)
            return new[]
            {
                int.Parse(time.Substring(0, 2)),
                int.Parse(time.Substring(2, 2))
            };

        return new int[0];
    }

    public static bool IsValidTime(int hour, int minute)
    {
        if ((hour < 0 && hour > 23) || (minute < 0 && minute > 59))
            return false;

        return true;
    }

    public static bool IsValidTime(this string time)
    {
        if (time.Length == 5)
            time.TimeNoFormat();

        if (time.Length != 4)
            return false;

        var hour = int.Parse(time.Substring(0, 2));
        var minute = int.Parse(time.Substring(2, 2));

        return IsValidTime(hour, minute);
    }

    public static string TimeFormat(int hour, int minute)
    {
        var strHour = hour.ToString().PadLeft(2, '0');
        var strMinute = minute.ToString().PadLeft(2, '0');

        return strHour + ":" + strMinute;
    }

    public static string TimeFormat(this string time)
    {
        if (time.Length != 4)
            return "";

        var hour = time.Substring(0, 2);
        var minute = time.Substring(2, 2);

        return hour + ":" + minute;
    }

    public static string TimeNoFormat(this string time)
    {
        return time.Replace(":", "");
    }
}
