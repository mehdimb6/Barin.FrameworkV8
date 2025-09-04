#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

#endregion

namespace Barin.Framework.Common.Helpers;

public static class ObjectExtension
{
    public static IList<Type> GetTypeHierarchy(this object instance)
    {
        var types = new List<Type>();

        var type = instance.GetType();
        while (type != typeof (object))
        {
            if (type != null) type = type.BaseType;
            types.Add(type);
        }
        return types;
    }

    public static object GetPropertyValue(this object instance, string member)
    {
        if (String.IsNullOrEmpty(member))
            return instance;

        if (instance != null) 
        {
            var columns = member.Split(new[] {'.'});

            var objectValue = instance; // instance cant be null if we are in if statement SO object value cant be null
            foreach (var s in columns)
            {
                var type = objectValue.GetType();
                var propertyInfo = type.GetPropertyInfo(s);
                if (propertyInfo == null)
                    return null;
                if (propertyInfo.DeclaringType != null && !propertyInfo.DeclaringType.IsAssignableFrom(type))
                {
                    type = type.BaseType;
                    propertyInfo = type.GetPropertyInfo(s);
                }
                var value = propertyInfo.GetValue(objectValue, null);
                objectValue = value;

                if (objectValue == null) // SO this if will never execute
                    return null;
            }
            return objectValue;
        }
        return null;  // this will never execute
    }

    public static void SetPropertyValue(this object instance, string propertyName, object value)
    {
        instance.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance).SetValue(
            instance, value, null);
    }

    public static void SetPropertyValue(this object instance, string member, object value, bool inner)
    {
        if (String.IsNullOrEmpty(member))
            return;

        if (instance != null)
        {
            if (member.Contains("."))
            {
                var columns = member.Split(new[] {'.'});
                var objectValue = instance;
                for (var i = 0; i < columns.Length - 1; i++)
                {
                    var propName = columns[i].ToString(CultureInfo.InvariantCulture);
                    var propertyInfo = objectValue.GetType().GetProperty(propName);
                    var propValue = propertyInfo.GetValue(objectValue, null);
                    if (propValue == null)
                    {
                        var o = Activator.CreateInstance(propertyInfo.PropertyType);
                        propertyInfo.SetValue(objectValue, o, null);
                        objectValue = o;
                    }
                    else
                    {
                        objectValue = propValue;
                    }
                }
                var property = objectValue.GetType().GetProperty(columns[columns.Length - 1]);
                property.SetValue(objectValue, value, null);
            }
            else
            {
                var type = instance.GetType();
                var propertyInfo = type.GetPropertyInfo(member);
                propertyInfo.SetValue(instance, value, null);
            }
        }
    }

    public static T CastTo<T>(this Object value, T targetType)
    {
        // targetType above is just for compiler magic
        // to infer the type to cast x to
        return (T)value;
    }
}