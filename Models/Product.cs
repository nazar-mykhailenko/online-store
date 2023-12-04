using System.ComponentModel.DataAnnotations;

namespace HouseholdOnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Image { get; set; }

        [Required]
        public int Left { get; set; }

        [Required]
        public int Price { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }

        public ICollection<Characterictic>? Characterictics { get; set; }
    }

    public class Characterictic
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Value { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
