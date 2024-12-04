using POS_Frontend.Models;
using POS_Frontend.Models.Auth;

namespace POS_Frontend.Services.Interfaces;

public interface IAuthService
{
    public Task<ResponseVm?> Login(LoginRequestVm loginRequestVm);
}