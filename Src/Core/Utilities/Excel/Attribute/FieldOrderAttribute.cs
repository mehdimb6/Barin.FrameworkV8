using System;

namespace Barin.Framework.Utilities.Excel.Attribute;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class FieldOrderAttribute : System.Attribute
{
    public string Column { get; set; }
}
