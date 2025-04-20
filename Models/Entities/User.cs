using System.ComponentModel.DataAnnotations;

public class UserReg
{
    [Key]
    public int Id { get; set; } 
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string UserType { get; set; }
}
