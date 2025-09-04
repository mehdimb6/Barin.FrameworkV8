namespace Barin.Framework.Common.Enums;

public enum DateMode
{
    /// <summary>
    /// M - Y
    /// </summary>
    Month = 0,

    /// <summary>
    /// YYYY/MM/DD
    /// </summary>
    Day = 1,

    /// <summary>
    ///YYYY/MM/DD - HH:mm
    /// </summary>
    DateAndTime = 2,

    /// <summary>
    /// h:mm
    /// </summary>
    Time = 3,

    /// <summary>
    /// {روز هفته} DD {نام ماه} YYYY
    /// </summary>
    Full = 4,

    /// <summary>
    /// {روز هفته} DD {نام ماه} YYYY
    /// </summary>
    FullDate = 5,

    /// <summary>
    /// {روز هفته} DD {نام ماه} YYYY HH:mm
    /// </summary>
    FullDateTime = 6,

    /// <summary>
    /// {روز هفته} YYYY/M/D 
    /// </summary>
    DateAndWeekdayName = 7,

    /// <summary>
    /// YYYY/MM/DD
    /// </summary>
    Today = 8,

    /// <summary>
    /// YYYY/MM
    /// </summary>
    YearAndMonth = 9
}
