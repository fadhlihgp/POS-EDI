namespace POS_Backend.ViewModels;

public class ErrorResponseVm
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
}

public class SingleDataResponse
{
    public string Message { get; set; }
    public object Data { get; set; }
}

public class MultipleDataResponse
{
    public string Message { get; set; }
    public object Data { get; set;}
    public int TotalData { get; set; }
}

public class SingleDataResponseOther<T>
{
    public string Message { get; set; }
    public T Data { get; set; }
}
public class NoDataResponse
{
    public string Message { get; set; }
}