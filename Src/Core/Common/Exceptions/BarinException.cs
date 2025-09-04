namespace Barin.Framework.Common.Exceptions;

public class BarinException : Exception
{
    public BarinException()
    {
    }

    public BarinException(string message) : base(message)
    {
    }

    public BarinException(string message, Exception inner) : base(message, inner)
    {
    }
}
