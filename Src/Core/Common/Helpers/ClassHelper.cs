namespace Barin.Framework.Common.Helpers;

public class ClassHelper
{
    /// <summary>
    /// بررسی وجود یک خصوصیت در کلاس
    /// </summary>
    public bool HasProperty<TEntity>(string name)
    {
        name = name.Trim();

        var objType = typeof(TEntity);
        var properties = objType.GetProperties();

        foreach (var property in properties)
        {
            if (property.Name == name) return true;
        }

        return false;
    }

    /// <summary>
    /// بررسی وجود یک فیلد در کلاس
    /// </summary>
    public bool HasField<TEntity>(string name)
    {
        name = name.Trim();

        var objType = typeof(TEntity);
        var fields = objType.GetFields();

        foreach (var property in fields)
        {
            if (property.Name == name) return true;
        }

        return false;
    }
}
