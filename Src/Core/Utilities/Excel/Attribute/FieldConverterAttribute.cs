using System;

namespace Barin.Framework.Utilities.Excel.Attribute;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class FieldConverterAttribute : System.Attribute
{
    public Type Type { get; set; }
}