using System.Configuration;

namespace Barin.Framework.Common.Config;

public static class AppSettings
{
    public static string? DefaultResetPassword => ConfigurationManager.AppSettings["DefaultResetPassword"];
}
