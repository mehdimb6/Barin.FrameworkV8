using Barin.Framework.Common.Enums;
using Barin.Framework.Common.Exceptions;
using System.Globalization;

namespace Barin.Framework.Common.Helpers;

public static class DateTimeHelper
{
    #region Fields & Properties

    private static readonly string[] persianMonthNames =
    {
        "فروردین",
        "اردیبهشت",
        "خرداد",
        "تیر",
        "مرداد",
        "شهریور",
        "مهر",
        "آبان",
        "آذر",
        "دی",
        "بهمن",
        "اسفند"
    };

    private static readonly string[] persianDaysOfWeek =
    {
        "شنبه",
        "یکشنبه",
        "دوشنبه",
        "سه شنبه",
        "چهارشنبه",
        "پنجشنبه",
        "جمعه"
    };

    private static Calendar _persianCalendar;
    private static Calendar PersianCalendar => _persianCalendar ?? (_persianCalendar = GetCalendar(CalendarType.Persian));


    private static Calendar _gregorianCalendar;
    private static Calendar GregorianCalendar => _gregorianCalendar ?? (_gregorianCalendar = GetCalendar(CalendarType.Gregorian));


    private static Calendar _hijriCalendar;
    private static Calendar HijriCalendar => _hijriCalendar ?? (_hijriCalendar = GetCalendar(CalendarType.Hijri));

    public static DateTime MinDateTime = new DateTime(1753, 1, 1);

    public static string[] SplitDateTimeChar => new[] { "/", "\\", ":", "-", " " };
    public static string RightNow => $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";

    public static int Today
    {
        get
        {
            var pc = new PersianCalendar();
            var now = DateTime.Now;

            var year = pc.GetYear(now).ToString().PadLeft(4, '0');
            var month = pc.GetMonth(now).ToString().PadLeft(2, '0');
            var day = pc.GetDayOfMonth(now).ToString().PadLeft(2, '0');

            return int.Parse($"{year}{month}{day}");
        }
    }

    #endregion

    #region Public Methods

    public static string ToPersianDate(this DateTime date)
    {
        if (date == DateTime.MinValue || date == DateTime.MaxValue)
            return "";

        var pCalendar = new PersianCalendar();

        var pYear = pCalendar.GetYear(date);
        var pMonth = pCalendar.GetMonth(date);
        var pDay = pCalendar.GetDayOfMonth(date);

        return $"{pYear as object:d2}/{(object)pMonth:d2}/{(object)pDay:d2}";
    }

    public static DateTime ToDateTime(this string persianDate)
    {
        var pCalendar = new PersianCalendar();
        var strArray = persianDate.Split(SplitDateTimeChar, StringSplitOptions.RemoveEmptyEntries);
        var year = int.Parse(strArray[0]);
        var month = int.Parse(strArray[1]);
        var day = int.Parse(strArray[2]);

        return pCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
    }

    /// <summary>
    /// دریافت تقویم های مختلف
    /// </summary>
    private static Calendar GetCalendar(CalendarType calendarType)
    {
        switch (calendarType)
        {
            case CalendarType.Persian:
                return new PersianCalendar();

            case CalendarType.Gregorian:
                return new GregorianCalendar();

            case CalendarType.Hijri:
                return new HijriCalendar();

            default:
                return new PersianCalendar();
        }
    }

    /// <summary>
    /// دریافت تاریخ شمسی با فرمت های مختلف
    /// </summary>      
    public static string GetPersianDate(this DateTime dateTime, DateMode mode)
    {
        if (!IsInValidRange(dateTime))
            return "تاریخ میلادی وارد شده در محدوده مجاز نمی باشد";

        var calendar = new PersianCalendar();
        if (calendar.MinSupportedDateTime > dateTime || calendar.MaxSupportedDateTime < dateTime)
            return dateTime.ToString(CultureInfo.InvariantCulture);

        switch (mode)
        {
            case DateMode.YearAndMonth:
                return $@"{calendar.GetYear(dateTime):0000}/{calendar.GetMonth(dateTime):00}";

            case DateMode.Month:
                return $@"{persianMonthNames[calendar.GetMonth(dateTime) - 1]} - {calendar.GetYear(dateTime)}";

            case DateMode.Day:
            case DateMode.Today:
                return $@"{calendar.GetYear(dateTime):0000}/{calendar.GetMonth(dateTime):00}/{calendar.GetDayOfMonth(dateTime):00}";

            case DateMode.DateAndTime:
                return $@"{calendar.GetYear(dateTime):0000}/{calendar.GetMonth(dateTime):00}/{calendar.GetDayOfMonth(dateTime):00} - {calendar.GetHour(dateTime):00}:{calendar.GetMinute(dateTime):00}";

            case DateMode.Full:
            case DateMode.FullDate:
                return $@"{GetPersianDaysOfWeekName(calendar.GetDayOfWeek(dateTime))} {calendar.GetDayOfMonth(dateTime)} {GetPersianMonthName(dateTime)} {calendar.GetYear(dateTime)}";

            case DateMode.FullDateTime:
                return $@"{GetPersianDaysOfWeekName(calendar.GetDayOfWeek(dateTime))} {calendar.GetDayOfMonth(dateTime)} {GetPersianMonthName(dateTime)} {calendar.GetYear(dateTime)} {calendar.GetHour(dateTime)}:{calendar.GetMinute(dateTime):00}";

            case DateMode.Time:
                return $@"{calendar.GetHour(dateTime)}:{calendar.GetMinute(dateTime):00}";

            case DateMode.DateAndWeekdayName:
                return $@"{GetPersianDaysOfWeekName(calendar.GetDayOfWeek(dateTime))} {calendar.GetYear(dateTime):0000}/{calendar.GetMonth(dateTime)}/{calendar.GetDayOfMonth(dateTime)}";

            default:
                return string.Empty;
        }
    }

    /// <summary>
    /// دریافت تاریخ شمسی روز جاری
    /// </summary>
    public static string GetCurrentPersianDate() => GetPersianDate(DateTime.Now, DateMode.DateAndTime);

    /// <summary>
    /// دریافت عدد شمسی روز جاری
    /// </summary>
    public static int GetPersianDay(this DateTime dateTime) => PersianCalendar.GetDayOfMonth(dateTime);

    /// <summary>
    /// دریافت عدد شمسی ماه جاری
    /// </summary>
    public static int GetPersianMonth(this DateTime dateTime) => PersianCalendar.GetMonth(dateTime);

    /// <summary>
    /// دریافت عدد شمسی سال جاری
    /// </summary>
    public static int GetPersianYear(this DateTime dateTime) => PersianCalendar.GetYear(dateTime);

    /// <summary>
    /// دریافت عدد روز جاری در هفته
    /// </summary>
    public static int GetPersianDaysOfWeekNumber(this DateTime dateTime) => GetPersianDaysOfWeekNumber(PersianCalendar.GetDayOfWeek(dateTime));

    /// <summary>
    /// دریافت عدد روز جاری در هفته
    /// </summary>
    public static int GetPersianDaysOfWeekNumber(this DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Saturday:
                return 1;

            case DayOfWeek.Sunday:
                return 2;

            case DayOfWeek.Monday:
                return 3;

            case DayOfWeek.Tuesday:
                return 4;

            case DayOfWeek.Wednesday:
                return 5;

            case DayOfWeek.Thursday:
                return 6;

            case DayOfWeek.Friday:
                return 7;

            default:
                return 0; // This will never happen
        }
    }

    /// <summary>
    /// دریافت متن فارسی روز جاری در هفته
    /// </summary>
    public static string GetPersianDaysOfWeekName(this DateTime dateTime) => GetPersianDaysOfWeekName(PersianCalendar.GetDayOfWeek(dateTime));

    /// <summary>
    /// دریافت متن فارسی روز جاری در هفته
    /// </summary>
    public static string GetPersianDaysOfWeekName(this DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Saturday:
                return persianDaysOfWeek[0];

            case DayOfWeek.Sunday:
                return persianDaysOfWeek[1];

            case DayOfWeek.Monday:
                return persianDaysOfWeek[2];

            case DayOfWeek.Tuesday:
                return persianDaysOfWeek[3];

            case DayOfWeek.Wednesday:
                return persianDaysOfWeek[4];

            case DayOfWeek.Thursday:
                return persianDaysOfWeek[5];

            case DayOfWeek.Friday:
                return persianDaysOfWeek[6];

            default:
                return string.Empty; // This will never happen
        }
    }

    /// <summary>
    /// گرفتن تمام روزهای ماه شمسی به صورت خروجی میلادی - ورودی به صورت میلادی
    /// </summary>
    public static List<DateTime> GetPersianDaysOfMonth(this DateTime dateTime)
    {
        var daysOfMonth = new List<DateTime>();
        var persianCalendar = new PersianCalendar();
        var month = persianCalendar.GetMonth(dateTime);

        var prevDay = dateTime;
        var nextDay = dateTime;
        while (persianCalendar.GetMonth(prevDay) == month || persianCalendar.GetMonth(nextDay) == month)
        {
            if (persianCalendar.GetMonth(prevDay) == month)
            {
                if (!daysOfMonth.Contains(prevDay))
                    daysOfMonth.Add(prevDay);
                prevDay = prevDay.Subtract(new TimeSpan(1, 0, 0, 0));
            }
            if (persianCalendar.GetMonth(nextDay) == month)
            {
                if (!daysOfMonth.Contains(nextDay))
                    daysOfMonth.Add(nextDay);
                nextDay = nextDay.AddDays(1);
            }
        }
        daysOfMonth.Sort();
        return daysOfMonth;
    }

    /// <summary>
    /// گرفتن اولین روز ماه شمسی به صورت خروجی میلادی - ورودی به صورت میلادی
    /// </summary>
    public static DateTime GetFirstDayInMonth(this DateTime date)
    {
        var year = GetPersianYear(date.Date);
        var month = GetPersianMonth(date.Date);
        var persianDate = $"{year}/{month.ToString().PadLeft(2, '0')}/01";
        var dateTime = ToMiladi(persianDate);
        return dateTime;
    }

    /// <summary>
    /// گرفتن آخرین روز ماه شمسی به صورت خروجی میلادی - ورودی به صورت میلادی
    /// </summary>
    public static DateTime GetEndDayInMonth(this DateTime date)
    {
        var year = GetPersianYear(date.Date);
        var month = GetPersianMonth(date.Date);
        var day = GetDaysInMonth(date, CalendarType.Context);
        var persianDate = $"{year}/{month.ToString().PadLeft(2, '0')}/{day.ToString().PadLeft(2, '0')}";
        var dateTime = ToMiladi(persianDate);
        return dateTime;
    }

    /// <summary>
    /// تبدیل تاریخ شمسی به میلادی
    /// </summary>
    public static DateTime ToMiladi(this string persianDate, string seperator = "/")
    {
        persianDate = persianDate.Replace(seperator, "");

        if (persianDate.Length != 8)
            throw new BarinException("تاریخ وارد شده نامعتبر است");

        var flag = int.TryParse(persianDate, out int result);
        if (!flag)
            throw new BarinException("تاریخ وارد شده نامعتبر است");

        var year = int.Parse(persianDate.Substring(0, 4));
        var month = byte.Parse(persianDate.Substring(4, 2));
        var day = byte.Parse(persianDate.Substring(6, 2));

        return ToMiladi(year, month, day);
    }

    /// <summary>
    /// تبدیل تاریخ شمسی به میلادی
    /// </summary>
    public static DateTime ToMiladi(int persianYear, byte persianMonth, byte persianDay) => new DateTime(persianYear, persianMonth, persianDay, PersianCalendar);

    /// <summary>
    /// دریافت تهی در صورت نبودن تاریخ در محدوده مجاز 
    /// </summary>
    public static DateTime? ToNullable(this DateTime dateTime)
    {
        if (dateTime <= MinDateTime || dateTime == DateTime.MaxValue)
            return null;

        return dateTime;
    }

    /// <summary>
    /// دریافت بیشترین تاریخ در صورت تهی بودن 
    /// </summary>
    public static DateTime ToMaxIfNull(this DateTime? dateTime) => dateTime ?? DateTime.MaxValue;

    /// <summary>
    /// دریافت کمترین تاریخ در صورت تهی بودن 
    /// </summary>
    public static DateTime ToMinIfNull(this DateTime? dateTime) => dateTime ?? MinDateTime;

    /// <summary>
    /// دریافت نام فارسی ماه ها
    /// </summary>
    public static string GetPersianMonthName(this DateTime date) => GetPersianMonthName(PersianCalendar.GetMonth(date));

    /// <summary>
    /// دریافت نام فارسی ماه ها
    /// </summary>
    public static string GetPersianMonthName(this byte month) => GetPersianMonthName((int)month);

    /// <summary>
    /// دریافت نام فارسی ماه ها
    /// </summary>
    public static string GetPersianMonthName(this short month) => GetPersianMonthName((int)month);

    /// <summary>
    /// دریافت نام فارسی ماه ها
    /// </summary>
    public static string GetPersianMonthName(this long month) => GetPersianMonthName((int)month);

    /// <summary>
    /// دریافت نام فارسی ماه ها
    /// </summary>
    public static string GetPersianMonthName(this int month) => month > 0 && month < 13 ? persianMonthNames[month - 1] : string.Empty;

    /// <summary>
    /// دریافت آخرین لحظه روز جاری
    /// </summary>
    public static DateTime GetLastMomentOfDay(this DateTime dateTime) => dateTime.Date.AddDays(1).AddTicks(-1);

    /// <summary>
    /// افزایش یا کاهش تعداد روز به تاریخ وارد شده
    /// </summary>
    public static DateTime AddDaysToDate(this DateTime dateTime, int days = 1) => GetCalendar(CalendarType.Gregorian).AddDays(dateTime, days);

    /// <summary>
    /// افزایش یا کاهش تعداد هفته به تاریخ وارد شده
    /// </summary>
    public static DateTime AddWeeksToDate(this DateTime dateTime, int weeks = 1) => GetCalendar(CalendarType.Gregorian).AddWeeks(dateTime, weeks);

    /// <summary>
    /// افزایش یا کاهش تعداد ماه به تاریخ وارد شده
    /// </summary>
    public static DateTime AddMonthsToDate(this DateTime dateTime, int months = 1) => GetCalendar(CalendarType.Context).AddMonths(dateTime, months);

    /// <summary>
    /// افزایش یا کاهش تعداد سال به تاریخ وارد شده
    /// </summary>
    public static DateTime AddYearsToDate(this DateTime dateTime, int years = 1) => GetCalendar(CalendarType.Context).AddYears(dateTime, years);

    /// <summary>
    /// دریافت چندمین روز ماه در تاریخ ورودی با توجه به نوع تقویم
    /// </summary>
    public static int GetDayOfMonth(this DateTime dateTime, CalendarType calendarType) => GetCalendar(calendarType).GetDayOfMonth(dateTime);

    /// <summary>
    /// دریافت تعداد روز ماه در تاریخ ورودی با توجه به نوع تقویم
    /// </summary>
    public static int GetDaysInMonth(this DateTime dateTime, CalendarType calendarType)
    {
        var calendar = GetCalendar(calendarType);
        return calendar.GetDaysInMonth(calendar.GetYear(dateTime), calendar.GetMonth(dateTime));
    }

    /// <summary>
    /// دریافت چندمین روز سال در تاریخ ورودی با توجه به نوع تقویم
    /// </summary>
    public static int GetDayOfYear(this DateTime dateTime, CalendarType calendarType) => GetCalendar(calendarType).GetDayOfYear(dateTime);

    /// <summary>
    /// دریافت چندمین ماه سال در تاریخ ورودی با توجه به نوع تقویم
    /// </summary>
    public static int GetMonthOfYear(this DateTime dateTime, CalendarType calendarType) => GetCalendar(calendarType).GetMonth(dateTime);

    /// <summary>
    /// دریافت تعداد روزهای سال در تاریخ ورودی با توجه به نوع تقویم
    /// </summary>
    public static int GetDaysInYear(this DateTime dateTime, CalendarType calendarType)
    {
        var calendar = GetCalendar(calendarType);
        return calendar.GetDaysInYear(calendar.GetYear(dateTime));
    }

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی
    /// در صورت وجود جداکننده در تاریخ آن را در ورودی مشخص کنید
    /// </summary>
    public static string GetPersianDate(this string gregorianDate, string seperator = "")
    {
        if (string.IsNullOrEmpty(gregorianDate))
            return null;

        if (seperator.Length != 0)
            gregorianDate = gregorianDate.Replace(seperator, "");

        if (gregorianDate.Length != 8 || !int.TryParse(gregorianDate, out int result))
            return null;

        var enYear = int.Parse(gregorianDate.Substring(0, 4));
        var enMonth = int.Parse(gregorianDate.Substring(4, 2));
        var enDay = int.Parse(gregorianDate.Substring(6, 2));
        var dt = new DateTime(enYear, enMonth, enDay);

        var pc = new PersianCalendar();
        var faYear = pc.GetYear(dt).ToString();
        var faMonth = pc.GetMonth(dt).ToString();
        var faDay = pc.GetDayOfMonth(dt).ToString();

        var strDate = faYear + seperator + (faMonth.Length == 1 ? "0" + faMonth : faMonth) + seperator + (faDay.Length == 1 ? "0" + faDay : faDay);
        return strDate;
    }

    /// <summary>
    /// تبدیل تاریخ شمسی به میلادی
    /// در صورت وجود جداکننده در تاریخ آن را در ورودی مشخص کنید
    /// </summary>
    public static string GetGregorianDate(this string persianDate, string seperator = "")
    {
        if (string.IsNullOrEmpty(persianDate))
            return null;

        if (seperator.Length != 0)
            persianDate = persianDate.Replace(seperator, "");

        if (persianDate.Length != 8 || !int.TryParse(persianDate, out int result))
            return null;

        var faYear = int.Parse(persianDate.Substring(0, 4));
        var faMonth = int.Parse(persianDate.Substring(4, 2));
        var faDay = int.Parse(persianDate.Substring(6, 2));

        var pc = new PersianCalendar();
        var dt = new DateTime(faYear, faMonth, faDay, pc);

        var enYear = dt.Year.ToString();
        var enMonth = dt.Month.ToString();
        var enDay = dt.Day.ToString();

        var strDate = enYear + seperator + (enMonth.Length == 1 ? "0" + enMonth : enMonth) + seperator + (enDay.Length == 1 ? "0" + enDay : enDay);
        return strDate;
    }

    /// <summary>
    /// اعتبار سنجی تاریخ میلادی یا شمسی  یا ...
    /// بستگی به نوع تاریخ ورودی
    /// </summary>
    public static bool IsValidDate(this DateTime dateTime, CalendarType calendarType = CalendarType.Persian) => GetCalendar(calendarType).MinSupportedDateTime <= dateTime && GetCalendar(calendarType).MaxSupportedDateTime >= dateTime;

    /// <summary>
    /// اعتبار سنجی تاریخ میلادی
    /// </summary>
    public static bool IsValidDate(int[] value) => DateTime.TryParse($"{value[0]}-{value[1]}-{value[2]}", out DateTime dateTime);

    /// <summary>
    /// اعتبار سنجی تاریخ میلادی
    /// </summary>
    public static bool IsValidDate(this int date) => IsValidDate(date.SplitDate());

    /// <summary>
    /// اعتبار سنجی تاریخ
    /// </summary>
    public static bool IsValidPersianDate(int year, byte month, byte day)
    {
        if (year < 1300 || year > 1450)
            return false;

        if (month < 1 || month > 12)
            return false;

        if ((day < 1 || day > 31) && month <= 6)
            return false;

        if ((day < 1 || day > 30) && month > 6)
            return false;

        return true;
    }

    /// <summary>
    /// اعتبار سنجی تاریخ
    /// </summary>
    public static bool IsValidPersianDate(this string date, char seperator = '/')
    {
        if (date == null)
            return false;

        date = date.Replace(seperator.ToString(), "");

        if (date.Length != 8)
            return false;

        var year = int.Parse(date.Substring(0, 4));
        var month = byte.Parse(date.Substring(4, 2));
        var day = byte.Parse(date.Substring(6, 2));

        return IsValidPersianDate(year, month, day);
    }

    /// <summary>
    /// بررسی تاریخ ورودی در بازه تاریخی مجاز
    /// 03/21/0622 00:00:00 (Gregorian date) and 12/31/9999
    /// </summary>       
    public static bool IsInValidRange(DateTime? date)
    {
        var startDate = new DateTime(0622, 03, 21);
        var endDate = new DateTime(9999, 12, 31);

        if (date > startDate && date < endDate)
            return true;

        return false;
    }

    /// <summary>
    /// تبدیل متن ماه فارسی به عدد ماه
    /// </summary>
    public static byte ConvertMonthNameToNumber(this string monthName)
    {
        switch (monthName)
        {
            case "فروردین": return 1;
            case "اردیبهشت": return 2;
            case "خرداد": return 3;
            case "تیر": return 4;
            case "مرداد": return 5;
            case "شهریور": return 6;
            case "مهر": return 7;
            case "آبان": return 8;
            case "آذر": return 9;
            case "دی": return 10;
            case "بهمن": return 11;
            case "اسفند": return 12;
        }

        return 0;
    }

    /// <summary>
    /// Index=0 >> اولین روز ماه,
    /// Index=1 >> آخرین روز ماه,
    /// Index=2 >> روز جاری
    /// </summary>
    public static string[] GetFirstAndLastAndCurrentDayInMonthPersian()
    {
        var pc = new PersianCalendar();
        var thisDate = DateTime.Now;

        var year = pc.GetYear(thisDate);
        var month = pc.GetMonth(thisDate);
        var day = pc.GetDayOfMonth(thisDate);
        var dayCount = pc.GetDaysInMonth(year, month, 1);

        var result = new[]
        {
            DateFormat(year, month, 1, ""),
            DateFormat(year, month, dayCount, ""),
            DateFormat(year, month, day, "")
        };

        return result;
    }

    /// <summary>
    /// Index=0 >> اولین روز سال,
    /// Index=1 >> آخرین روز سال,
    /// Index=2 >> روز جاری
    /// </summary>
    public static string[] GetFirstAndLastAndCurrentDayInYearPersian()
    {
        var pc = new PersianCalendar();
        var thisDate = DateTime.Now;

        var year = pc.GetYear(thisDate);
        var month = pc.GetMonth(thisDate);
        var day = pc.GetDayOfMonth(thisDate);
        var dayCount = pc.GetDaysInYear(year, 1);

        var result = new[]
        {
            DateFormat(year, 1, 1, ""),
            DateFormat(year, 12, dayCount == 365 ? 29 : 30, ""),
            DateFormat(year, month, day, "")
        };

        return result;
    }

    /// <summary>
    /// اضافه کردن جدا کننده به تاریخ ورودی بدون جدا کننده
    /// </summary>
    public static string DateFormat(this int date, string seperator = "/") => DateFormat(date.ToString(), seperator);

    /// <summary>
    /// اضافه کردن جدا کننده به تاریخ ورودی بدون جدا کننده
    /// </summary>
    public static string DateFormat(int year, byte month, byte day, string seperator = "/")
    {
        return $"{year.ToString().PadLeft(4, '0')}{seperator}{month.ToString().PadLeft(2, '0')}{seperator}{day.ToString().PadLeft(2, '0')}";
    }

    /// <summary>
    /// اضافه کردن جدا کننده به تاریخ ورودی بدون جدا کننده
    /// </summary>
    public static string DateFormat(int year, int month, int day, string seperator = "/") => DateFormat(year, (byte)month, (byte)day, seperator);

    /// <summary>
    /// اضافه کردن جدا کننده به تاریخ ورودی بدون جدا کننده
    /// </summary>
    public static string DateFormat(this string date, string seperator = "/")
    {
        date = date.Trim();

        if (date.Length != 8 || !int.TryParse(date, out int result))
            return "";

        var year = date.Substring(0, 4);
        var month = date.Substring(4, 2);
        var day = date.Substring(6, 2);

        return $"{year}{seperator}{month}{seperator}{day}";
    }

    /// <summary>
    /// جدا کردن روز و ماه و سال از تاریخ ورودی 
    /// </summary>
    public static int[] SplitDate(this string date, string seperator = "/")
    {
        date = date.Trim().Replace(seperator.ToString(), "");

        if (date.Length != 8 || !int.TryParse(date, out int result))
            return new int[0];

        return new[]
        {
            int.Parse(date.Substring(0, 4)),
            int.Parse(date.Substring(4, 2)),
            int.Parse(date.Substring(6, 2))
        };
    }

    /// <summary>
    /// جدا کردن روز و ماه و سال از تاریخ ورودی 
    /// </summary>
    public static int[] SplitDate(this int date) => SplitDate(date.ToString());

    /// <summary>
    /// حذف جدا کننده از تاریخ ورودی با جدا کننده
    /// </summary>
    public static int DateNoFormat(this string date, string seperator = "/")
    {
        var temp = date.Trim().Replace(seperator.ToString(), "");
        if (temp.IsNullOrWhiteSpace())
            return 0;

        return int.Parse(temp);
    }

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی و نمایش به صورت تمام متنی
    /// </summary>
    public static string ToPersianChequeFormat(this DateTime dateTime)
    {
        var pc = new PersianCalendar();
        var pYear = pc.GetYear(dateTime);
        var pMonth = pc.GetMonth(dateTime);
        var pDay = pc.GetDayOfMonth(dateTime);

        return string.Format("{0} {1} {2}", PersianHelper.ToWord(pDay, true), persianMonthNames[pMonth - 1], PersianHelper.ToWord(pYear));
    }

    /// <summary>
    /// تبدیل تاریخ عددی شمسی به تاریخ متنی شمسی
    /// </summary>
    public static string PersianDateDetail(this int date)
    {
        var strDate = date.ToString();
        var day = "";
        var month = "";
        var year = "";

        if (strDate.Length == 6)
        {
            year = strDate.Substring(0, 4);
            month = strDate.Substring(4, 2);
        }
        else if (strDate.Length == 8)
        {
            year = strDate.Substring(0, 4);
            month = strDate.Substring(4, 2);
            day = strDate.Substring(5, 2);
        }
        else
            return "Invalid Format";

        var strMonth = GetPersianMonthName(byte.Parse(month));

        return string.Format("{0} {1} {2}", year, strMonth, day);
    }

    #endregion
}
