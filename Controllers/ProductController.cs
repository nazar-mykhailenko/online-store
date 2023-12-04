using HouseholdOnlineStore.Docs;
using HouseholdOnlineStore.Interfaces;
using HouseholdOnlineStore.Models;
using HouseholdOnlineStore.Models.DocumentModels;
using HouseholdOnlineStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace HouseholdOnlineStore.Controllers
{
	public class ProductController : Controller
	{
		IProductRepository _repo;
		private readonly IPhotoService _photoService;

		public ProductController(IProductRepository repo, IPhotoService photoService)
        {
            _repo = repo;
			_photoService = photoService;
		}
        public IActionResult Index(string search, ProductFilters filters)
		{
			var list = _repo.GetAll(search, filters);
			return View(list);
		}
		[Authorize(Roles ="Admin")]
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(CreateProductVM productVM)
		{
			if (!ModelState.IsValid)
			{
				return View(productVM);
			}
            var itemsToRemove = productVM.Characterictics.Where(c => string.IsNullOrEmpty(c.Name) || string.IsNullOrEmpty(c.Value)).ToList();
            foreach (var item in itemsToRemove)
            {
                productVM.Characterictics.Remove(item);
            }
            var result = _photoService.AddPhoto(productVM.Image);
			var product = new Product
			{
				Name = productVM.Name,
				Description = productVM.Description,
				Manufacturer = productVM.Manufacturer,
				Left = productVM.Left,
				Price = productVM.Price,
				Image = result.Url.ToString(),
				Feedbacks = productVM.Feedbacks,
				Characterictics = productVM.Characterictics
			};
			_repo.Add(product);
			return RedirectToAction("Index");
		}
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
		{
			var prod = _repo.GetByIdWithCharacteristics(id);
			if (prod == null)
			{
				return NotFound();
			}
			var prodVM = new EditProductVM
			{
				Id = prod.Id,
				Name = prod.Name,
				Description = prod.Description,
				Manufacturer = prod.Manufacturer,
				Left = prod.Left,
				Price = prod.Price,
				Feedbacks = prod.Feedbacks,
				Characterictics = prod.Characterictics ?? new List<Characterictic>()
			};

			return View(prodVM);
		}

		[HttpPost]
		public IActionResult Edit(EditProductVM prodVM)
		{
			if (!ModelState.IsValid)
			{
				return View(prodVM);
			}
            var itemsToRemove = prodVM.Characterictics.Where(c => string.IsNullOrEmpty(c.Name) || string.IsNullOrEmpty(c.Value)).ToList();
            foreach (var item in itemsToRemove)
            {
                prodVM.Characterictics.Remove(item);
            }
			var prod = _repo.GetByIdWithCharacteristicsLoaded(prodVM.Id);
			string imageUrl = prod.Image;
			if (prodVM.Image != null)
            {
				
				if(!string.IsNullOrEmpty(prod.Image))
				{
					var fi = new FileInfo(imageUrl);

					var publicId = Path.GetFileNameWithoutExtension(fi.Name);
					_photoService.DeletePhoto(publicId);
				}
				

				imageUrl = _photoService.AddPhoto(prodVM.Image).Url.ToString();
			}

			prod.Name = prodVM.Name;
			prod.Description = prodVM.Description;
			prod.Manufacturer = prodVM.Manufacturer;
			prod.Left = prodVM.Left;
			prod.Price = prodVM.Price;
			prod.Feedbacks = prodVM.Feedbacks;
			prod.Characterictics = prodVM.Characterictics;
			prod.Image = imageUrl;

			_repo.Update(prod);
			return RedirectToAction("Index");
		
		}
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
		{
			var prod = _repo.GetByIdWithCharacteristics(id);
			if (prod == null) 
			{
				return NotFound();
			}

			return View(prod);
		}

		[HttpPost]
		public IActionResult Delete(Product product)
		{
			product = _repo.GetById(product.Id);
			if (product.Image != null)
			{
				var fi = new FileInfo(product.Image);

				var publicId = Path.GetFileNameWithoutExtension(fi.Name);
				_photoService.DeletePhoto(publicId);
			}

			_repo.Delete(product);
			return RedirectToAction("Index");
		}

		public IActionResult Details(int id)
		{
			var prod = _repo.GetByIdWithCharacteristics(id);
			if (prod == null)
			{
				return NotFound();
			}

			return View(prod);
		}

		[Authorize(Roles ="Admin")]
		public IActionResult GenerateReport()
		{
			var list = _repo.GetAll();

			var model = new ProductsReportModel()
			{
				Items = list
			};

			var document = new ProductsReportDocument(model);

			var bytes = document.GeneratePdf();
			var ms = new MemoryStream(bytes);
			return File(ms, "application/octet-stream", $"Report on {DateTime.Now}.pdf");
		}
	}
}
