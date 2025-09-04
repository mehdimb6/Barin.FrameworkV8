using System.Reflection;

namespace Barin.Framework.Common.Helpers;

public static class ConvertHelper
{
    public static byte[] GetBytes(this string str)
    {
        var bytes = new byte[str.Length * sizeof(char)];
        Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static string GetString(this byte[] bytes)
    {
        var chars = new char[bytes.Length / sizeof(char)];
        Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    public static TEntity Convert<TEntity, TDestination>(this TEntity dpm, TDestination dp, params string[] excludes) where TEntity : class
    {
        Type srcType = dp.GetType();
        Type desType = dpm.GetType();

        IList<PropertyInfo> srcProps = new List<PropertyInfo>(srcType.GetProperties());
        IList<PropertyInfo> desProps = new List<PropertyInfo>(desType.GetProperties());

        foreach (PropertyInfo sprop in srcProps)
        {
            foreach (PropertyInfo dprop in desProps)
            {
                if ((sprop.Name == dprop.Name) && (!excludes.Any(s => s.Equals(sprop.Name))))
                {
                    dprop.SetValue(dpm, sprop.GetValue(dp, null));
                }
            }
        }

        return dpm;
    }
}
