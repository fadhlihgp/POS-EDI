namespace POS_Backend.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string? message) : base(message)
    {
    }
}