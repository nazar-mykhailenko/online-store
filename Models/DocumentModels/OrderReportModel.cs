using System.ComponentModel.DataAnnotations;
using System.Net;

namespace HouseholdOnlineStore.Models.DocumentModels
{
	public class OrderReportModel
	{
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

        public int UkrPostCount { get; set; }
		public int NovaPostCount { get; set; }

		public int MeestCount { get; set; }

        public int CardCount { get; set; }
        public int CashCount { get; set; }
        public List<Order> Items { get; set; }
	}
}
