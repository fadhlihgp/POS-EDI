namespace POS_Backend.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string? message) : base(message)
    {
    }
}