using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Barin.Framework.Common.Helpers;

public static class EnumHelper<T>
     where T : struct, Enum // This constraint requires C# 7.3 or later.
{
    public static T ParseEnum(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static List<EnumValue> GetValues()
    {
        List<EnumValue> values = new List<EnumValue>();
        foreach (var itemType in Enum.GetValues(typeof(T)))
        {
            //For each value of this enumeration, add a new EnumValue instance
            values.Add(new EnumValue()
            {
                Name = Enum.GetName(typeof(T), itemType),
                Value = (int)itemType
            });
        }

        return values;
    }

    public static IList<T> GetValues(Enum value)
    {
        var enumValues = new List<T>();

        foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
        }
        return enumValues;
    }

    public static T Parse(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static IList<string> GetNames(Enum value)
    {
        return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
    }

    public static IList<string> GetDisplayValues(Enum value)
    {
        return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
    }

    private static string lookupResource(Type resourceManagerProvider, string resourceKey)
    {
        var resourceKeyProperty = resourceManagerProvider.GetProperty(resourceKey,
            BindingFlags.Static | BindingFlags.Public, null, typeof(string),
            new Type[0], null);
        if (resourceKeyProperty != null)
        {
            return (string)resourceKeyProperty.GetMethod.Invoke(null, null);
        }

        return resourceKey; // Fallback with the key name
    }

    public static string GetDisplayValue(T value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        var descriptionAttributes = fieldInfo.GetCustomAttributes(
            typeof(DisplayAttribute), false) as DisplayAttribute[];

        if (descriptionAttributes[0].ResourceType != null)
            return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

        if (descriptionAttributes == null) return string.Empty;
        return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
    }

    public static string GetName(T value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        return fieldInfo.Name;
    }
}

public static class EnumHelper
{
    public static string GetDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if (attributes != null && attributes.Any())
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }

    public static List<string> GetDescriptionList(Type enumType)
    {
        var result = new List<string>();
        foreach (Enum item in Enum.GetValues(enumType))
            result.Add(GetDescription(item));

        return result;
    }

    public static Dictionary<int, string> GetDescriptionDictionary(Type enumType)
    {
        int[] ids = (int[])Enum.GetValues(enumType);
        int counter = 0;

        var result = new Dictionary<int, string>();
        foreach (Enum item in Enum.GetValues(enumType))
        {
            result.Add(ids[counter], GetDescription(item));
            counter++;
        }

        return result;
    }
}

public class EnumValue
{
    public string Name { get; set; }
    public int Value { get; set; }
}
