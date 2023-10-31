namespace Customer.Management.Api.Entities;

using System.Text.Json.Serialization;

public class UserApi
{
    public int Id { get; set; }
    public string? Username { get; set; }

    [JsonIgnore]
    public string? PasswordHash { get; set; }
}


