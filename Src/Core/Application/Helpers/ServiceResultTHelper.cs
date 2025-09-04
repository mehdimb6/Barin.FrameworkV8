using Barin.Framework.Application.BaseModels;
using Barin.Framework.Application.Enums;
using Barin.Framework.Common;

namespace Barin.Framework.Application.Helpers;

public partial class ServiceResultTHelper
{
    /// <summary>
    /// برای زمانی که عملیات با موفقیت انجام شده است
    /// </summary>
    public static ServiceResult<TResult> Success<TResult>(TResult result, string message = "")
    {
        return new ServiceResult<TResult>
        {
            Status = ResultStatus.Success,
            Message = message,
            Result = result
        };
    }

    /// <summary>
    /// برای زمانی که عملیات با موفقیت انجام شده است
    /// </summary>
    public static ServiceResult<bool> Success(string message = "")
    {
        return new ServiceResult<bool>
        {
            Status = ResultStatus.Success,
            Message = message,
            Result = true
        };
    }

    /// <summary>
    /// برای زمانی که می خواهیم هشدار دهیم
    /// </summary>
    public static ServiceResult<TResult> Warning<TResult>(TResult result, string message)
    {
        return new ServiceResult<TResult>
        {
            Status = ResultStatus.Warning,
            Message = message,
            Result = result
        };
    }

    /// <summary>
    /// برای زمانی که می خواهیم هشدار دهیم
    /// </summary>
    public static ServiceResult<TResult> Warning<TResult>(string message)
    {
        return new ServiceResult<TResult>
        {
            Status = ResultStatus.Warning,
            Message = message
        };
    }

    /// <summary>
    /// برای زمانی که می خواهیم هشدار دهیم
    /// </summary>
    public static ServiceResult<bool> Warning(string message)
    {
        return new ServiceResult<bool>
        {
            Status = ResultStatus.Warning,
            Message = message,
            Result = false
        };
    }

    /// <summary>
    /// برای زمانی که خطایی اتفاق می افتد
    /// </summary>
    public static ServiceResult<TResult> Error<TResult>(string message = BasePublicMessage.ErrorSystem)
    {
        return new ServiceResult<TResult>
        {
            Status = ResultStatus.Danger,
            Message = message
        };
    }

    /// <summary>
    /// برای زمانی که خطایی اتفاق می افتد
    /// </summary>
    public static ServiceResult<bool> Error(string message = BasePublicMessage.ErrorSystem)
    {
        return new ServiceResult<bool>
        {
            Status = ResultStatus.Danger,
            Message = message,
            Result = false
        };
    }
}