using System.ComponentModel.DataAnnotations;

namespace HouseholdOnlineStore.Models.ViewModels
{
	public class ProductsQuantityVM
	{
		public Product Prod { get; set; }

		[Required]
		public int Quantity { get; set; }
	}
}
