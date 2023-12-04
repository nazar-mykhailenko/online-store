using HouseholdOnlineStore.Data;
using HouseholdOnlineStore.Interfaces;
using HouseholdOnlineStore.Models;
using HouseholdOnlineStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HouseholdOnlineStore.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly AppDBContext _context;

		public OrderRepository(AppDBContext context)
		{
			_context = context;
		}
		public bool Add(Order order)
		{
			_context.Orders.Add(order);
			return Save();
		}

		public bool Delete(Order order)
		{
			_context.Orders.Remove(order);
			return Save();
		}

		public IEnumerable<Order> GetAll(OrderFilters filters)
		{
			var list =  _context.Orders.AsQueryable();

            if (filters.MinDate != null)
            {
				list = list.Where(x => x.OrderDateTime > filters.MinDate);
            }

			if (filters.MaxDate != null)
			{
				list = list.Where(x => x.OrderDateTime < filters.MaxDate);
			}

			if (filters.DeliveryType != null)
			{
				list = list.Where(x => x.DeliveryType == filters.DeliveryType);
			}

			if (filters.PaymentType != null)
			{
				list = list.Where(x => x.DeliveryType == filters.PaymentType);
			}

			if (filters.Status != null)
			{
				list = list.Where(x => x.Status == filters.Status);
			}

            if (filters.Name != null)
            {
				list = list.Where(x => x.Name.Contains(filters.Name));
			}

			if (filters.Surname != null)
			{
				list = list.Where(x => x.Surname.Contains(filters.Surname));
			}

            if (filters.PhoneNumber != null)
            {
				list = list.Where(x => x.PhoneNumber.Contains(filters.PhoneNumber));
            }

			return list.ToList().OrderByDescending(x => x.OrderDateTime);
        }

		public List<Order> GetAllWithProducts(OrderFilters filters)
		{
			var list = _context.Orders.Include(o => o.Products).AsQueryable();

			if (filters.MinDate != null)
			{
				list = list.Where(x => x.OrderDateTime >= filters.MinDate);
			}

			if (filters.MaxDate != null)
			{
				list = list.Where(x => x.OrderDateTime <= filters.MaxDate);
			}

			if (filters.DeliveryType != null)
			{
				list = list.Where(x => x.DeliveryType == filters.DeliveryType);
			}

			if (filters.PaymentType != null)
			{
				list = list.Where(x => x.PaymentType == filters.PaymentType);
			}

			if (filters.Status != null)
			{
				list = list.Where(x => x.Status == filters.Status);
			}

			if (filters.Name != null)
			{
				list = list.Where(x => x.Name.Contains(filters.Name));
			}

			if (filters.Surname != null)
			{
				list = list.Where(x => x.Surname.Contains(filters.Surname));
			}

			if (filters.PhoneNumber != null)
			{
				list = list.Where(x => x.PhoneNumber.Contains(filters.PhoneNumber));
			}

			return list.ToList();
		}

		public Order GetById(int id)
		{
			return _context.Orders.FirstOrDefault(o => o.Id == id);
		}

		public int GetPriceById(int id)
		{
			return _context.Orders.FirstOrDefault(o => o.Id == id).Products.Select(x => x.Price * x.Quantity).Sum();
		}

		public Order GetByIdWithProducts(int id)
		{
			return _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == id);
		}

		public IEnumerable<Order> GetByUserId(int userId)
		{
			return _context.Orders.Where(o => o.AppUserId == userId);
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0;
		}

		public bool Update(Order order)
		{
			_context.Orders.Update(order);
			return Save();
		}
	}
}
