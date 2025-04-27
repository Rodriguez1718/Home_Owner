using System.Collections.Generic;

public class EditUserViewModel
{
    public string? Id { get; set; }

    public required string FullName { get; set; }

    public required string Email { get; set; }

    public List<string> Roles { get; set; } = new();

    public List<string> AllRoles { get; set; } = new(); // For dropdown/checklist
}
