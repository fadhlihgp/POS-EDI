using System.ComponentModel.DataAnnotations;

namespace POS_Frontend.Models.Auth;

public class LoginRequestVm
{
    [Required(ErrorMessage = "Username or Email must not be empty")]
    public string Username { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class LoginResponseVm
{
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Token { get; set; }
}