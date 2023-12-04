using System.ComponentModel.DataAnnotations;

namespace HouseholdOnlineStore.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        [Required]
        public short Rate { get; set; }
        public string? Text { get; set; }
        [Required]
        public AppUser Author { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}