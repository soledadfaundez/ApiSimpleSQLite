namespace Customer.Management.Api.Models.Customer;

using System.ComponentModel.DataAnnotations;


public class CreateRequest
{
    
    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    [EnumDataType(typeof(Entities.CustomerType))]
    public string? Type { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    public string? Password { get; set; }

    [Required]
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}