using Barin.Framework.Application.BaseModels;
using Barin.Framework.Application.Enums;
using Barin.Framework.Common;

namespace Barin.Framework.Application.Helpers;

public partial class ServiceResultHelper
{
    /// <summary>
    /// برای زمانی که عملیات با موفقیت انجام شده است
    /// </summary>
    public static ServiceResult Success(object result, string message = "")
    {
        return new ServiceResult
        {
            Status = ResultStatus.Success,
            Message = message,
            Result = result
        };
    }

    /// <summary>
    /// برای زمانی که عملیات با موفقیت انجام شده است
    /// </summary>
    public static ServiceResult Success(string message = "")
    {
        return new ServiceResult
        {
            Status = ResultStatus.Success,
            Message = message,
            Result = true
        };
    }

    /// <summary>
    /// برای زمانی که می خواهیم هشدار دهیم
    /// </summary>
    public static ServiceResult Warning(object result, string message)
    {
        return new ServiceResult
        {
            Status = ResultStatus.Warning,
            Message = message,
            Result = result
        };
    }

    /// <summary>
    /// برای زمانی که می خواهیم هشدار دهیم
    /// </summary>
    public static ServiceResult Warning(string message)
    {
        return new ServiceResult
        {
            Status = ResultStatus.Warning,
            Message = message,
            Result = false
        };
    }

    /// <summary>
    /// برای زمانی که خطایی اتفاق می افتد
    /// </summary>
    public static ServiceResult Error(string message = BasePublicMessage.ErrorSystem)
    {
        return new ServiceResult
        {
            Status = ResultStatus.Danger,
            Message = message,
            Result = false
        };
    }
}