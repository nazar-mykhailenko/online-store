using HouseholdOnlineStore.Models;
using HouseholdOnlineStore.Models.ViewModels;
using System.Diagnostics.Eventing.Reader;

namespace HouseholdOnlineStore.Interfaces
{
	public interface IOrderRepository
	{
		public IEnumerable<Order> GetAll(OrderFilters filters);
		public List<Order> GetAllWithProducts(OrderFilters filters);
		public IEnumerable<Order> GetByUserId(int userId);
		public Order GetById(int id);
		public int GetPriceById(int id);
		public Order GetByIdWithProducts(int id);
		public bool Add(Order order);
		public bool Update(Order order);
		public bool Delete(Order order);
		public bool Save();
	}
}
