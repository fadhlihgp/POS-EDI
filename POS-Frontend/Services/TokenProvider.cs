﻿using POS_Frontend.Helpers;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Services;

public class TokenProvider : ITokenProvider
{
    private IHttpContextAccessor _contextAccessor;

    public TokenProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public void SetToken(string token)
    {
        _contextAccessor.HttpContext?.Response.Cookies.Append(StaticData.TokenCookie, token);
    }

    public string? GetToken()
    {
        string? token = null;
        bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticData.TokenCookie, out token);
        return hasToken is true ? token : null;
    }

    public void ClearToken()
    {
        _contextAccessor.HttpContext?.Response.Cookies.Delete(StaticData.TokenCookie);
    }
}