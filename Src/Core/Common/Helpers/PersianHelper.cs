using Barin.Framework.Common.Enums;

namespace Barin.Framework.Common.Helpers;

public static class PersianHelper
{
    #region Fields & Properties

    private static readonly Dictionary<int, string> PersianNumberStrings = new Dictionary<int, string>
        {
            {1, "یک"},
            {2, "دو"},
            {3, "سه"},
            {4, "چهار"},
            {5, "پنج"},
            {6, "شش"},
            {7, "هفت"},
            {8, "هشت"},
            {9, "نه"},
            {10, "ده"},
            {11, "یازده"},
            {12, "دوازده"},
            {13, "سیزده"},
            {14, "چهارده"},
            {15, "پانزده"},
            {16, "شانزده"},
            {17, "هفده"},
            {18, "هجده"},
            {19, "نوزده"},
            {20, "بیست"},
            {30, "سی"},
            {40, "چهل"},
            {50, "پنجاه"},
            {60, "شصت"},
            {70, "هفتاد"},
            {80, "هشتاد"},
            {90, "نود"},
            {100, "یکصد"},
            {200, "دویست"},
            {300, "سیصد"},
            {400, "چهارصد"},
            {500, "پانصد"},
            {600, "ششصد"},
            {700, "هفتصد"},
            {800, "هشتصد"},
            {900, "نهصد"},
        };

    private static List<decimal>? _threeDigit;

    #endregion

    #region Private Methods

    private static void DigitSeprator(decimal num)
    {
        if (num <= 0) return;
        if (_threeDigit == null)
            _threeDigit = new List<decimal>();

        _threeDigit.Add(num % 1000);
        num /= 1000;
        DigitSeprator(Math.Floor(num));
    }

    private static string DigitToWord(int num)
    {
        var result = string.Empty;
        var td = num % 100;
        if (td != 0)
        {
            if (td < 21)
            {
                result = PersianNumberStrings[td];
            }
            else
            {
                var i = td % 10;
                result = string.Format("{0}{1} {2}", PersianNumberStrings[td - i], i > 0 ? " و" : string.Empty, i > 0 ? PersianNumberStrings[i] : string.Empty);
            }
        }
        num = num - td;
        if (num > 0)
            result = string.Format("{0}{1} {2}", PersianNumberStrings[num], td > 0 ? " و" : string.Empty, result);
        return result;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// تبدیل کاراکترهای ی و ک عربی به فارسی و بلعکس
    /// </summary>
    public static string YeKafSafe(this string term, CharacterConflictType characterConflict)
    {
        if (characterConflict == CharacterConflictType.ToPersian)
            return string.IsNullOrWhiteSpace(term) ? term : term.Trim().Replace("ي", "ی").Replace("ك", "ک");

        if (characterConflict == CharacterConflictType.ToArabic)
            return string.IsNullOrWhiteSpace(term) ? term : term.Trim().Replace("ی", "ي").Replace("ک", "ك");

        return term;
    }

    /// <summary>
    /// تبدیل کاراکترهای ی و ک فارسی به عربی
    /// </summary>
    public static string FixPersianCharsToArabic(this string term)
    {
        return YeKafSafe(term, CharacterConflictType.ToArabic);
    }

    /// <summary>
    /// تبدیل کاراکترهای ی و ک فارسی به عربی
    /// </summary>
    public static string ChangePersianCharacterConflictToArabic(this string term)
    {
        return YeKafSafe(term, CharacterConflictType.ToArabic);
    }

    /// <summary>
    /// تبدیل کاراکترهای ی و ک عربی به فارسی
    /// </summary>
    public static string FixArabicCharsToPersian(this string term)
    {
        return YeKafSafe(term, CharacterConflictType.ToPersian);
    }

    /// <summary>
    /// تبدیل کاراکترهای ی و ک عربی به فارسی
    /// </summary>
    public static string ChangeArabicCharacterConflictToPersian(this string term)
    {
        return YeKafSafe(term, CharacterConflictType.ToPersian);
    }

    /// <summary>
    /// گرفتن متن فارسی ترتیب 
    /// </summary>
    /// <param name="number">1,2,3,4,5,6,7,8,9</param>
    /// <returns>متن فارسی  ترتیبی معادل عدد</returns>
    public static string GetOrdinal(int number)
    {
        switch (number)
        {
            case 1: return "اول";
            case 2: return "دوم";
            case 3: return "سوم";
            case 4: return "چهارم";
            case 5: return "پنجم";
            case 6: return "ششم";
            case 7: return "هفتم";
            case 8: return "هشتم";
            case 9: return "نهم";
            case 10: return "دهم";
            default: return number.ToString();
        }
    }

    /// <summary>
    /// تبدیل اعداد انگلیسی به فارسی
    /// </summary>
    public static string En2FaNumber(this string str)
    {
        return str.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").Replace("5", "۵").Replace("6", "۶").Replace("7", "۷").Replace("8", "۸").Replace("9", "۹");
    }

    /// <summary>
    /// تبدیل اعداد فارسی به انگلیسی
    /// </summary>
    public static string Fa2EnNumber(this string str)
    {
        return str.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
    }

    /// <summary>
    /// بررسی فارسی بودن متن
    /// </summary>
    public static bool IsFarsi(this string Str)
    {
        //var allowChr = new char[47] { 'آ', 'ا', 'ب', 'پ', 'ت', 'ث', 'ج', 'چ', 'ح', 'خ', 'د', 'ذ', 'ر', 'ز', 'ژ', 'س', 'ش', 'ص', 'ض', 'ط', 'ظ', 'ع', 'غ', 'ف', 'ق', 'ک', 'گ', 'ل', 'ا', 'ن', 'و', 'ه', 'ی', 'ة', 'ي', 'ؤ', 'إ', 'أ', 'ء', 'ئ', 'ۀ', ' ', 'ك', 'ﮎ', 'ﮏ', 'ﮐ', 'ﮑ' };
        var allowChar = new int[47] { 1590, 1589, 1579, 1602, 1601, 1594, 1593, 1607, 1582, 1581, 1580, 1670, 1662, 1588, 1587, 1740, 1576, 1604, 1575, 1578, 1606, 1605, 1705, 1711, 1592, 1591, 1586, 1585, 1584, 1583, 1574, 1608, 1577, 1610, 1688, 1572, 1573, 1571, 1569, 1570, 1728, 32, 1603, 64398, 64399, 64400, 64401 };
        foreach (char item in Str)
        {
            var ascii = (int)item;
            if (!allowChar.Contains(ascii))
                return false;
        }

        return true;
    }

    /// <summary>
    /// تبدیل عدد به حروف
    /// </summary>
    public static string ToWord(this byte num, bool isDay = false)
    {
        return ToWord((decimal)num, isDay);
    }

    /// <summary>
    /// تبدیل عدد به حروف
    /// </summary>
    public static string ToWord(this short num, bool isDay = false)
    {
        return ToWord((decimal)num, isDay);
    }

    /// <summary>
    /// تبدیل عدد به حروف
    /// </summary>
    public static string ToWord(this int num, bool isDay = false)
    {
        return ToWord((decimal)num, isDay);
    }

    /// <summary>
    /// تبدیل عدد به حروف
    /// </summary>
    public static string ToWord(this long num, bool isDay = false)
    {
        return ToWord((decimal)num, isDay);
    }

    /// <summary>
    /// تبدیل عدد به حروف
    /// </summary>
    public static string ToWord(this float num, bool isDay = false)
    {
        return ToWord((decimal)num, isDay);
    }

    /// <summary>
    /// تبدیل عدد به حروف
    /// </summary>
    public static string ToWord(this double num, bool isDay = false)
    {
        return ToWord((decimal)num, isDay);
    }

    /// <summary>
    /// تبدیل عدد به حروف
    /// </summary>
    public static string ToWord(this decimal num, bool isDay = false)
    {
        _threeDigit = new List<decimal>();
        DigitSeprator(num);

        var ds = new[] { "", "هزار", "میلیون", "میلیارد", "هزار میلیارد", "صدهزار میلیارد" };
        var result = string.Empty;
        for (var i = 0; i < _threeDigit.Count; i++)
        {
            if (_threeDigit[i] == 0) continue;
            var threeDigit = Convert.ToInt32(_threeDigit[i]);

            var va = _threeDigit[i] > 0 && i > 0 && result.Length > 0 ? " و " : string.Empty;
            var ps = DigitToWord(threeDigit);
            if (isDay)
            {
                if (_threeDigit[i] == 3) ps = "سو";
                ps += "م";
            }

            result = string.Format("{0} {1}{2}{3}", ps, ds[i], va, result);
        }
        return result;
    }

    /// <summary>
    /// تبدیل بولین به متن فارسی
    /// </summary>
    public static string ToPersian(this bool value, BoolType type = BoolType.Bali)
    {
        if (type == BoolType.Bali && value)
            return "بلی";

        if (type == BoolType.Bali && !value)
            return "خیر";

        if (type == BoolType.Darad && value)
            return "دارد";

        if (type == BoolType.Darad && !value)
            return "ندارد";

        if (type == BoolType.Hast && value)
            return "هست";

        if (type == BoolType.Hast && !value)
            return "نیست";

        return "";
    }

    #endregion
}