namespace POS_Backend.Exceptions;

public class InternalServerError : Exception
{
    public InternalServerError(string? message) : base(message)
    {
    }
}