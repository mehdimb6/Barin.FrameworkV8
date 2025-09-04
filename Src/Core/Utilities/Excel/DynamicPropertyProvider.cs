namespace Barin.Framework.Utilities.Excel;

public class DynamicPropertyProvider : IPropertyProvider
{
    private readonly List<PropertyColumn> _columns;
    public DynamicPropertyProvider(List<PropertyColumn> columns)
    {
        _columns = columns;
    }
    public List<OrderedPropertyDescriptor> GetProperties<T>(T entity) where T : new()
    {
        var result = new List<OrderedPropertyDescriptor>();
        var t = new T();

        if (typeof(T) == typeof(Dictionary<string, object>))
        {
            var dictionary = entity as Dictionary<string, object>;
            var cols = dictionary.Keys.ToArray();

            foreach (var col in cols)
            {
                var columnProperty = _columns.FirstOrDefault(c => c.Name == col);
                if (columnProperty == null) continue;

                result.Add(new OrderedPropertyDescriptor
                {
                    Title = columnProperty.Title,
                    Order = columnProperty.Index,
                    Width = columnProperty.Width,
                    Name = col
                });
            }
        }
        else
        {
            var properties = t.GetType().GetProperties();

            foreach (var property in properties)
            {
                var columnProperty = _columns.FirstOrDefault(c => c.Name == property.Name);
                if (columnProperty == null) continue;

                result.Add(new OrderedPropertyDescriptor
                {
                    PropertyInfo = property,
                    Title = columnProperty.Title,
                    Order = columnProperty.Index,
                    Width = columnProperty.Width,
                    Name = property.Name

                });
            }
        }

        return result;
    }
}
