using System.ComponentModel.DataAnnotations;

namespace Customer.Management.Api.Models.User;

public class LoginModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6)]
    public string? Password { get; set; }
   
}