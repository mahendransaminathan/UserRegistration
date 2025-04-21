using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

public class UserReg
{
    [JsonProperty("id")]
    public required string Id { get; set; } 
    
    [JsonProperty("email")]
    public required string Email { get; set; }
    
    [JsonProperty("password")]
    public required string Password { get; set; }
    
    [JsonProperty("userType")]    
    public required string UserType { get; set; }
}
