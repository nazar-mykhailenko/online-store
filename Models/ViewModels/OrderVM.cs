using System.ComponentModel.DataAnnotations;

namespace HouseholdOnlineStore.Models.ViewModels
{
	public class OrderVM
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
		public ICollection<ProductsQuantityVM> Products { get; set; }

		[Required]
		public OrderStatus Status { get; set; }
	}
}
