using System.ComponentModel.DataAnnotations;

namespace Customer.Management.Api.Models.User;

public class RegisterModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}
