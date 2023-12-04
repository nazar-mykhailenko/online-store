using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseholdOnlineStore.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
