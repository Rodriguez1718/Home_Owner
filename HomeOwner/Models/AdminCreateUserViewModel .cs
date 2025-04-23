using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeOwner.ViewModels
{
    public class AdminCreateUserViewModel : RegisterViewModel
    {
        public List<string> AllRoles { get; set; } = new();
        public List<string> SelectedRoles { get; set; } = new();
    }
}
