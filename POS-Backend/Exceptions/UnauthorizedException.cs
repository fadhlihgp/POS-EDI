namespace POS_Backend.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string? message) : base(message)
    {
    }
}