using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HouseholdOnlineStore.Models.ViewModels
{
    public class EditUserVM
    {
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        [Required]
        public string Email { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
    }
}
