using System.ComponentModel.DataAnnotations;

namespace HouseholdOnlineStore.Models
{
    public enum OrderStatus
    {
        New,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Returned
    }

    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime? OrderDateTime { get; set; }
        public string? OrderNumber { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        public string DeliveryType { get; set; }
        [Required]
        public string PaymentType { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Surname { get; set; }
		[Required]
		public string PhoneNumber { get; set; }

		[Required]
        public ICollection<ProductsQuantity> Products { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public int? AppUserId { get; set; }

        
    }

    public class ProductsQuantity
    {
        public int Id { get; set; }

        [Required]
        public int ProdId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int OrderId { get; set; }
    }
}
