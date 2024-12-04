using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using POS_Frontend.Models.Auth;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;

    public AuthController(IAuthService authService, ITokenProvider tokenProvider)
    {
        _authService = authService;
        _tokenProvider = tokenProvider;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            // Redirect ke halaman lain jika user sudah login
            return RedirectToAction("Index", "Home");
        }
        LoginRequestVm loginRequestVm = new LoginRequestVm();
        return View(loginRequestVm);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestVm loginRequestVm)
    {
        // LoginResponseVm loginResponseVm = new LoginResponseVm();
        if (ModelState.IsValid)
        {
            var responseVm = await _authService.Login(loginRequestVm);
            if (responseVm != null && responseVm.IsSuccess)
            {
                LoginResponseVm loginResponseVm =
                    JsonConvert.DeserializeObject<LoginResponseVm>(Convert.ToString(responseVm.Data));

                await SignInUser(loginResponseVm);
                
                // Set token
                _tokenProvider.SetToken(loginResponseVm.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseVm.Message;
                return View(loginRequestVm);
            }
        }
        else
        {
            return View(loginRequestVm);
        }
        
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _tokenProvider.ClearToken();
        return RedirectToAction("Login");
    }
    
    private async Task SignInUser(LoginResponseVm loginResponseVm)
    {
        // Store token in claims
        var claims = new List<Claim>
        {
            new Claim("UserName", loginResponseVm.Username),
            new Claim("FullName", loginResponseVm.FullName),
            new Claim("AccessToken", loginResponseVm.Token)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Sign in with cookies
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}