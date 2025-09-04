using System;

namespace Barin.Framework.Utilities.Excel.Attribute;

[AttributeUsage(AttributeTargets.Class)]
public sealed class WorkSheetAttribute : System.Attribute
{
    public string SheetName { get; set; }
    public int StartRowNumber { get; set; }
}