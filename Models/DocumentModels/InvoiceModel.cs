using System.ComponentModel.DataAnnotations;
using System.Net;

namespace HouseholdOnlineStore.Models.DocumentModels
{
	public class InvoiceModel
	{
		public int InvoiceNumber { get; set; }
		public DateTime? OrderDate { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }

		public List<OrderItem> Items { get; set; }
	}

	public class OrderItem
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
