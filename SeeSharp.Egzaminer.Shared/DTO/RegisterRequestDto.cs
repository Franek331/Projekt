using System.ComponentModel.DataAnnotations;

namespace SeeSharp.Egzaminer.Shared.DTO;

public class RegisterRequestDto
{
    [Required]
    public string? UserName { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required]
    public string? PhoneNumber { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? Role { get; set; }
}