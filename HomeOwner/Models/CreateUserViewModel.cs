using System.ComponentModel.DataAnnotations;

public class CreateUserViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    // For displaying checkboxes
    public List<string> AllRoles { get; set; } = new();

    // For capturing selected roles
    public List<string> SelectedRoles { get; set; } = new();
}
