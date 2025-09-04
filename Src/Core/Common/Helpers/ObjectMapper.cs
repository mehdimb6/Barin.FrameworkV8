using Mapster;

namespace Barin.Framework.Common.Helpers;

/// <summary>
///  is an object-object mapper. Object-object mapping works by transforming an input object of one type into an output object of a different type.
///  این کلاس برای جلوگیری از وابستگی به یک کتابخانه(ابزار) طراحی شده. 
///  جهت مپ کردن آبجکت ها باید از این کلاس استفاده شود
/// </summary>
public static class ObjectMapper
{
    private static object _obj = new object();

    /// <summary>
    /// transform source object of TSource type into an output object of a TTarget type
    /// </summary>
    public static TTarget Map<TSource, TTarget>(TSource source)
    {
        lock (_obj)
        {
            return source.Adapt<TTarget>();
        }
    }

    /// <summary>
    /// transform source object into an output object of a TTarget type
    /// </summary>
    public static TTarget Map<TTarget>(object source)
    {
        return source.Adapt<TTarget>();
    }
}
