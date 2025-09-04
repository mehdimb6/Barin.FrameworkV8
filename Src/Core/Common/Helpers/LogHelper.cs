using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Targets;
using System.Diagnostics;
using System.Text.Json;

namespace Barin.Framework.Common.Helpers;

public static class LogHelper
{
    private static Logger logger = LogManager.GetCurrentClassLogger();
    private static string _location = "";
    private static string _requestHeader = "";
    private static string _requestBody = "";

    private static IConfiguration _configuration;

    static LogHelper()
    {
        // ایجاد یک ConfigurationBuilder  
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // مسیر پایه  
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // بارگذاری فایل appsettings.json  

        // ساخت IConfiguration  
        _configuration = configurationBuilder.Build();

        var databaseTarget = (DatabaseTarget)LogManager.Configuration.FindTargetByName("database");
        databaseTarget.ConnectionString = ConnectionString;
        LogManager.ReconfigExistingLoggers();
    }

    #region Properties

    public static string Location
    {
        get
        {
            var trace = new StackFrame(1).GetMethod();
            return $"{trace.DeclaringType.FullName}.{trace.Name}";
        }
    }

    #endregion

    #region Private Methods

    // به خاطر استفاده در سازنده استاتیک فقط یکبار اجرا می شود
    private static string? ConnectionString
    {
        get
        {
            var connectionStringName = _configuration["ConnectionStringName"];
            if (string.IsNullOrWhiteSpace(connectionStringName))
            {
                throw new ArgumentNullException("نام کانکشن استرینگ مشخص نیست");
            }

            var encryptedConnectionString = _configuration.GetConnectionString(connectionStringName);
            if (string.IsNullOrWhiteSpace(encryptedConnectionString))
            {
                throw new ArgumentNullException("کانکشن استرینگ موجود نیست");
            }

            var plainConnectionString = Security.Code.TripleDes.Decrypt(encryptedConnectionString);
            return plainConnectionString;
        }
    }

    private static string GetLocation()
    {
        var trace = new StackFrame(3).GetMethod();
        return $"{trace.DeclaringType.FullName}.{trace.Name}";
    }

    private static void SetCommonParam(string userId = "", string targetId = "", string history = "")
    {
        MappedDiagnosticsContext.Clear();
        MappedDiagnosticsContext.Set("userId", userId.ToLower());
        MappedDiagnosticsContext.Set("location", string.IsNullOrWhiteSpace(_location) ? GetLocation() : _location);
        MappedDiagnosticsContext.Set("targetId", targetId.ToLower());
        MappedDiagnosticsContext.Set("history", history);
        MappedDiagnosticsContext.Set("requestHeader", _requestHeader);
        MappedDiagnosticsContext.Set("requestBody", _requestBody);

        _location = ""; // Reset _location
        _requestHeader = ""; // Reset _requestHeader
        _requestBody = ""; // Reset _requestBody
    }

    #endregion

    public static void SetExtraInfo(string location, string requestHeader, string requestBody)
    {
        _location = location;
        _requestHeader = requestHeader;
        _requestBody = requestBody;
    }

    #region Info

    public static void Info(string message)
    {
        SetCommonParam();
        logger.Info(message);
    }

    public static void Info(string message, string userId)
    {
        SetCommonParam(userId);
        logger.Info(message);
    }

    public static void Info(string message, Exception exception, string userId)
    {
        SetCommonParam(userId);
        logger.Info(exception, message);
    }

    public static void Info(string message, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Info(message);
    }

    public static void Info(string message, Exception exception, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Info(exception, message);
    }

    public static void Info(string message, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Info(message);
    }

    public static void Info(string message, Exception exception, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Info(exception, message);
    }

    //public  void Info(string message, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Info(message, args);
    //}

    //public  void Info(string message, Exception exception, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Info(exception, message, args);
    //}

    #endregion

    #region Warn

    public static void Warn(string message)
    {
        SetCommonParam();
        logger.Warn(message);
    }

    public static void Warn(string message, string userId)
    {
        SetCommonParam(userId);
        logger.Warn(message);
    }

    public static void Warn(string message, Exception exception, string userId)
    {
        SetCommonParam(userId);
        logger.Warn(exception, message);
    }

    public static void Warn(string message, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Warn(message);
    }

    public static void Warn(string message, Exception exception, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Warn(exception, message);
    }

    public static void Warn(string message, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Warn(message);
    }

    public static void Warn(string message, Exception exception, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Warn(exception, message);
    }

    //public  void Warn(string message, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Warn(message, args);
    //}

    //public  void Warn(string message, Exception exception, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Warn(exception, message, args);
    //}

    #endregion

    #region Error

    public static void Error(string message)
    {
        SetCommonParam();
        logger.Error(message);
    }

    public static void Error(string message, string userId)
    {
        SetCommonParam(userId);
        logger.Error(message);
    }

    public static void Error(string message, Exception exception, string userId)
    {
        SetCommonParam(userId);
        logger.Error(exception, message);
    }

    public static void Error(string message, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Error(message);
    }

    public static void Error(string message, Exception exception, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Error(exception, message);
    }

    public static void Error(string message, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Error(message);
    }

    public static void Error(string message, Exception exception, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Error(exception, message);
    }

    //public  void Error(string message, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Error(message, args);
    //}

    //public  void Error(string message, Exception exception, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Error(exception, message, args);
    //}

    #endregion

    #region Fatal

    public static void Fatal(string message)
    {
        SetCommonParam();
        logger.Fatal(message);
    }

    public static void Fatal(string message, string userId)
    {
        SetCommonParam(userId);
        logger.Fatal(message);
    }

    public static void Fatal(string message, Exception exception, string userId)
    {
        SetCommonParam(userId);
        logger.Fatal(exception, message);
    }

    public static void Fatal(string message, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Fatal(message);
    }

    public static void Fatal(string message, Exception exception, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Fatal(exception, message);
    }

    public static void Fatal(string message, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Fatal(message);
    }

    public static void Fatal(string message, Exception exception, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Fatal(exception, message);
    }

    //public  void Fatal(string message, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Fatal(message, args);
    //}

    //public  void Fatal(string message, Exception exception, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Fatal(exception, message, args);
    //}

    #endregion

    #region Debug

    public static void Debug(string message)
    {
        SetCommonParam();
        logger.Debug(message);
    }

    public static void Debug(string message, string userId)
    {
        SetCommonParam(userId);
        logger.Debug(message);
    }

    public static void Debug(string message, Exception exception, string userId)
    {
        SetCommonParam(userId);
        logger.Debug(exception, message);
    }

    public static void Debug(string message, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Debug(message);
    }

    public static void Debug(string message, Exception exception, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Debug(exception, message);
    }

    public static void Debug(string message, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Debug(message);
    }

    public static void Debug(string message, Exception exception, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Debug(exception, message);
    }

    //public  void Debug(string message, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Debug(message, args);
    //}

    //public  void Debug(string message, Exception exception, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Debug(exception, message, args);
    //}

    #endregion

    #region Trace

    public static void Trace(string message)
    {
        SetCommonParam();
        logger.Trace(message);
    }

    public static void Trace(string message, string userId)
    {
        SetCommonParam(userId);
        logger.Trace(message);
    }

    public static void Trace(string message, Exception exception, string userId)
    {
        SetCommonParam(userId);
        logger.Trace(exception, message);
    }

    public static void Trace(string message, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Trace(message);
    }

    public static void Trace(string message, Exception exception, string userId, string targetId)
    {
        SetCommonParam(userId, targetId);
        logger.Trace(exception, message);
    }

    public static void Trace(string message, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Trace(message);
    }

    public static void Trace(string message, Exception exception, string userId, string targetId, object history)
    {
        var jsonHistory = JsonSerializer.Serialize(history);
        SetCommonParam(userId, targetId, jsonHistory);
        logger.Trace(exception, message);
    }

    //public  void Trace(string message, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Trace(message, args);
    //}

    //public  void Trace(string message, Exception exception, string userId, params object[] args)
    //{
    //    SetCommonParam(userId);
    //    logger.Trace(exception, message, args);
    //}

    #endregion
}