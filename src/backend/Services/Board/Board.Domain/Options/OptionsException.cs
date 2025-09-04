namespace Board.Domain.Options;

public class OptionsException : Exception
{
    public OptionsException()
    { }

    public OptionsException(string message)
    : base(message)
    {
    }

    public OptionsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
