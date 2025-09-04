using Barin.Framework.Utilities.Excel.Attribute;
using System.Reflection;

namespace Barin.Framework.Utilities.Excel;

public class AttributeBasedPropertyProvider : IPropertyProvider
{
    private static ExportableAttribute GetExportableInfo(MemberInfo type)
    {
        var attrs = System.Attribute.GetCustomAttributes(type);
        return attrs.OfType<ExportableAttribute>().FirstOrDefault();
    }

    public List<OrderedPropertyDescriptor> GetProperties<T>(T entity) where T : new()
    {
        var result = new List<OrderedPropertyDescriptor>();
        var t = new T();
        var properties = t.GetType().GetProperties().Where(x => x.GetCustomAttribute<ExportableAttribute>() != null);

        foreach (var property in properties)
        {
            var att = GetExportableInfo(property);
            result.Add(new OrderedPropertyDescriptor { PropertyInfo = property, Title = att.Title, Order = att.Order, Name = property.Name });
        }

        return result;
    }

    public List<OrderedPropertyDescriptor> GetProperties<T>() where T : new()
    {
        return GetProperties<T>(new T());
    }
}
