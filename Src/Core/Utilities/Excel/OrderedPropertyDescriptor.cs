using System.Reflection;

namespace Barin.Framework.Utilities.Excel;

public class OrderedPropertyDescriptor
{
    public PropertyInfo PropertyInfo { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public int Width { get; set; }
}
