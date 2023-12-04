namespace HouseholdOnlineStore.Models.DocumentModels
{
	public class ProductsReportModel
	{
        public ProductsReportModel()
        {
             ReportDate = DateTime.Now;
        }
        public DateTime ReportDate { get; set; }

        public IEnumerable<Product> Items { get; set; }
    }
}
