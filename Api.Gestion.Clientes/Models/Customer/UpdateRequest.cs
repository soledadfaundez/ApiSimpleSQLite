namespace Customer.Management.Api.Models.Customer;

using System.ComponentModel.DataAnnotations;

public class UpdateRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [EnumDataType(typeof(Entities.CustomerType))]
    public string? Type { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    // treat empty string as null for password fields to 
    // make them optional in front end apps
    private string? _password;
    [MinLength(6)]
    public string? Password
    {
        get => _password;
        set => _password = replaceEmptyWithNull(value);
    }

    private string? _confirmPassword;
    [Compare("Password")]
    public string? ConfirmPassword 
    {
        get => _confirmPassword;
        set => _confirmPassword = replaceEmptyWithNull(value);
    }

    // helpers

    private string? replaceEmptyWithNull(string? value)
    {
        // replace empty string with null to make field optional
        return string.IsNullOrEmpty(value) ? null : value;
    }
}