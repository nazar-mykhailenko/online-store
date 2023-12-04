using HouseholdOnlineStore.Models;
using HouseholdOnlineStore.Models.ViewModels;

namespace HouseholdOnlineStore.Interfaces
{
	public interface IProductRepository
	{
		public IEnumerable<Product> GetAll();
		public IEnumerable<Product> GetAll(string search, ProductFilters filters);

		public Product GetById(int id);
		public Product GetByIdNoTracking(int id);
		public Product GetByIdWithCharacteristics(int id);
        public Product GetByIdWithCharacteristicsLoaded(int id);
		public void UpdateWithoutSave(Product product);
        public bool Add(Product product);
		public bool Update(Product product);
		public bool Delete(Product product);
		public bool Save();
	}
}
