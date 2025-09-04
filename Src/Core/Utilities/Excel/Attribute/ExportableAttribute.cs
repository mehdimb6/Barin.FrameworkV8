using System;

namespace Barin.Framework.Utilities.Excel.Attribute;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ExportableAttribute : System.Attribute
{
    public string Title { get; set; }
    public int Order { get; set; }
}