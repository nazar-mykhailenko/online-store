namespace HouseholdOnlineStore.Models.ViewModels
{
	public class OrderFilters
	{
        public DateTime? MinDate { get; set; }
		public DateTime? MaxDate { get; set; }
		public string? DeliveryType { get; set; }
		public string? PaymentType { get; set; }
		public OrderStatus? Status { get; set; }
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? PhoneNumber { get; set; }
	}
}
