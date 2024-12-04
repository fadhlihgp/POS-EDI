using Microsoft.EntityFrameworkCore;
using POS_Backend.Context;
using POS_Backend.Entities;
using POS_Backend.Exceptions;
using POS_Backend.Modules.Auth._Dto;
using POS_Backend.Modules.Auth._Repository._Interface;
using POS_Backend.Security;

namespace POS_Backend.Modules.Auth._Repository;

public class AuthRepository : IAuthRepository
{
    private AppDbContext _context;
    private IJwtUtil _jwtUtil;

    public AuthRepository(AppDbContext appDbContext, IJwtUtil jwtUtil)
    {
        _context = appDbContext;
        _jwtUtil = jwtUtil;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        Account? findByUsernameOrUsername = await _context.Accounts.Where(a =>
                ((a.Username.ToLower().Equals( loginRequestDto.Username.ToLower()) ||
                  a.Username.ToLower().Equals( loginRequestDto.Username.ToLower()))))
            .FirstOrDefaultAsync();
        
        if (findByUsernameOrUsername == null) throw new UnauthorizedException("Username atau Password salah");
        
        bool isValid = BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, findByUsernameOrUsername.Password);
        if (!isValid) throw new UnauthorizedException("Username/Username atau Password salah");

        try
        {
            // responseVm.User = userLogin;
            var token = _jwtUtil.GenerateToken(findByUsernameOrUsername);
            return new LoginResponseDto()
            {
                Username = findByUsernameOrUsername.Username,
                FullName = findByUsernameOrUsername.FullName,
                Token = token
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}