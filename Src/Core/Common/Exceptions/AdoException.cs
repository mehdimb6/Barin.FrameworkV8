namespace Barin.Framework.Common.Exceptions;

public class AdoException : BarinException
{
    public AdoException()
    {
    }

    public AdoException(string message) : base(message)
    {
    }

    public AdoException(string message, Exception inner) : base(message, inner)
    {
    }
}
