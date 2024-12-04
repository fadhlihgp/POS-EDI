using POS_Backend.Modules.Auth._Dto;

namespace POS_Backend.Modules.Auth._Repository._Interface;

public interface IAuthRepository
{
    public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
}