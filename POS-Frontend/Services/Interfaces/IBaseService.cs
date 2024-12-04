using POS_Frontend.Models;

namespace POS_Frontend.Services.Interfaces;

public interface IBaseService
{
    Task<ResponseVm?> SendAsync(RequestVm requestVm, bool withBearer = true);
}