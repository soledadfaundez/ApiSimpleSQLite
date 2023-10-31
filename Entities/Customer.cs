namespace Customer.Management.Api.Entities;

using System.Text.Json.Serialization;

public class Customer
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public CustomerType Type { get; set; }

    [JsonIgnore]
    public string? PasswordHash { get; set; }
}