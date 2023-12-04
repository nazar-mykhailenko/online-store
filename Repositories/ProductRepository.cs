using HouseholdOnlineStore.Data;
using HouseholdOnlineStore.Interfaces;
using HouseholdOnlineStore.Models;
using HouseholdOnlineStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HouseholdOnlineStore.Repositories
{
    public class ProductRepository : IProductRepository
    {
        readonly AppDBContext _context;
        public ProductRepository(AppDBContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetAll()
        {
            var list = _context.Products;
            return list;
        }

		public IEnumerable<Product> GetAll(string search, ProductFilters filters)
		{
            var list = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(p => p.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(filters.Manufacturer))
            {
                list = list.Where(p => p.Manufacturer.Trim().ToLower().Contains(filters.Manufacturer.Trim().ToLower()));
            }

            if(filters.MinValue.HasValue)
            {
                list = list.Where(p => p.Price > filters.MinValue);
            }

            if (filters.MaxValue.HasValue)
            {
                list = list.Where(p => p.Price < filters.MaxValue);
            }

            return list.ToList();
		}

		public bool Add(Product product)
        {
            _context.Products.Add(product);
            return Save();
        }

        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0;
        }

        public Product GetById(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }

		public Product GetByIdNoTracking(int id)
        {
			return _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == id);
		}

		public Product GetByIdWithCharacteristics(int id)
		{
			return _context.Products.Include(x => x.Characterictics).First(x => x.Id == id);
		}

        public Product GetByIdWithCharacteristicsLoaded(int id)
        {
            var prod = _context.Products.Include(x => x.Characterictics).First(x => x.Id == id);
            _context.Entry(prod).Collection("Characterictics").Load();
            return prod;
        }

        public bool Update(Product product)
        {
            _context.Products.Update(product);
            return Save();
        }

        public bool Delete(Product product)
        {
            _context.Products.Remove(product);
            return Save();
        }

		public void UpdateWithoutSave(Product product)
		{
			_context.Products.Update(product);
		}

	}
}
