namespace Barin.Framework.Application.BaseModels;

public class Bindable
{
    public Bindable()
    {
    }

    public Bindable(string text, string value)
    {
        Text = text;
        Value = value;
    }

    public Bindable(string text, string value, bool selected)
    {
        Text = text;
        Value = value;
        Selected = selected;
    }

    public string? Text { get; set; }
    public string? Value { get; set; }
    public bool Selected { get; set; }
}
