using POS_Frontend.Helpers;
using POS_Frontend.Models;
using POS_Frontend.Models.Auth;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Services;

public class AuthService : IAuthService
{
    private ITokenProvider _tokenProvider;
    private IBaseService _baseService;

    public AuthService(IBaseService baseService, ITokenProvider tokenProvider)
    {
        _baseService = baseService;
        _tokenProvider = tokenProvider;
    }

    public async Task<ResponseVm?> Login(LoginRequestVm loginRequestVm)
    {
        return await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.POST,
            Url = "/api/v1/auth/login",
            Data = loginRequestVm
        }, false);
    }
    
}