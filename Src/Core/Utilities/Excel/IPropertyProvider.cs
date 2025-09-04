namespace Barin.Framework.Utilities.Excel;

public interface IPropertyProvider
{
    List<OrderedPropertyDescriptor> GetProperties<T>(T entity) where T : new();
}
