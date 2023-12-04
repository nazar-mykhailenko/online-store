using System.ComponentModel.DataAnnotations;

namespace HouseholdOnlineStore.Models.ViewModels
{
    public class EditProductVM
    {
        public EditProductVM()
        {
            this.Characterictics = new List<Characterictic>();
            this.Feedbacks = new List<Feedback>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required]
        public string Manufacturer { get; set; }
        public IFormFile? Image { get; set; }

        [Required]
        public int Left { get; set; }

        [Required]
        public int Price { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        [Required]
        public ICollection<Characterictic> Characterictics { get; set; }
    }
}
