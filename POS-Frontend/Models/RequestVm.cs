using POS_Frontend.Helpers;

namespace POS_Frontend.Models;

public class RequestVm
{
    public StaticData.ApiType ApiType { get; set; } = StaticData.ApiType.GET;
    public string Url { get; set; }
    public object Data { get; set; }
    public bool IsFormMultipart { get; set; } = false;
    public string AccessToken { get; set; }
}