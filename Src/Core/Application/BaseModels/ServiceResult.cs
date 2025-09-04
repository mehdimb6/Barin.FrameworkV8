using Barin.Framework.Application.Enums;

namespace Barin.Framework.Application.BaseModels;

/// <summary>
/// فرمت یکپارچه خروجی وب ای پی آی ها
/// </summary>
/// <typeparam name="T">نتیجه اصلی</typeparam>
public class ServiceResult<T>
{
    public ServiceResult()
    {
    }

    public ServiceResult(ResultStatus status, string message, T result)
    {
        Status = status;
        Message = message;
        Result = result;
    }

    /// <summary>
    /// وضعیت صحیح بودن اجرای درخواست
    /// </summary>
    public bool IsSuccess => Status == ResultStatus.Success;

    /// <summary>
    /// پیام
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// وضعیت درخواست
    /// </summary>
    public ResultStatus? Status { get; set; }

    /// <summary>
    /// نتیجه درخواست
    /// </summary>
    public T? Result { get; set; }
}

/// <summary>
/// فرمت یکپارچه خروجی وب ای پی آی ها
/// </summary>
/// <typeparam name="T">نتیجه اصلی</typeparam>
public class ServiceResultEx<T> : ServiceResult<T>
{
    public ServiceResultEx()
    {
    }

    public ServiceResultEx(ResultStatus status, string message, T result, object ExResult)
    {
        Status = status;
        Message = message;
        Result = result;
    }

    /// <summary>
    /// اطلاعات اضافی
    /// </summary>
    public object? ExResult { get; set; }
}

/// <summary>
/// فرمت یکپارچه خروجی وب ای پی آی ها
/// </summary>
public class ServiceResult : ServiceResult<object>
{
    public ServiceResult()
    {
    }

    public ServiceResult(ResultStatus status, string message, object result)
    {
        Status = status;
        Message = message;
        Result = result;
    }
}

/// <summary>
/// فرمت یکپارچه خروجی وب ای پی آی ها
/// </summary>
public class ServiceResultEx : ServiceResult
{
    public ServiceResultEx()
    {
    }

    public ServiceResultEx(ResultStatus status, string message, object result, object ExResult)
    {
        Status = status;
        Message = message;
        Result = result;
    }

    /// <summary>
    /// اطلاعات اضافی
    /// </summary>
    public object? ExResult { get; set; }
}