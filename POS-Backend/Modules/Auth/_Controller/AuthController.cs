using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using POS_Backend.Modules.Auth._Dto;
using POS_Backend.Modules.Auth._Repository._Interface;
using POS_Backend.ViewModels;

namespace POS_Backend.Modules.Auth._Controller;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private IAuthRepository _authRepository;
    
    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var data = await _authRepository.Login(loginRequest);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil login",
            Data = data
        });
    }
}