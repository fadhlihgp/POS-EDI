namespace POS_Backend.Modules.Auth._Dto;

public class LoginResponseDto
{
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Token { get; set; }
}