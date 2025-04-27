using System.ComponentModel.DataAnnotations;

public class CreateUserViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    public required string FullName { get; set; }

    [Required]
    public required string UserName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    // For displaying checkboxes
    public List<string> AllRoles { get; set; } = new();

    // For capturing selected roles
    public List<string> SelectedRoles { get; set; } = new();
}
