namespace POS_Frontend.Models;

public class ResponseApiVm
{
    public string Message { get; set; }
    public object? Data { get; set; }
}

public class ResponseVm
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
    public object? Data { get; set; }
}