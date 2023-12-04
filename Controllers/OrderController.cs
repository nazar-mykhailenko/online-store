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
    [Authorize]
    public class OrderController : Controller
    {
		const string cookiePrefix = "cart_";
		private readonly IProductRepository _prodRepo;
		private readonly IOrderRepository _orderRepo;

		public OrderController(IProductRepository prodRepo, IOrderRepository orderRepo)
        {
			_prodRepo = prodRepo;
			_orderRepo = orderRepo;
		}
        [Authorize]
        public IActionResult Index()
        {
			var id = int.Parse(User.FindFirst("Id")?.Value);
            var list = _orderRepo.GetByUserId(id).ToList();
			return View(list);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult List(OrderFilters filters)
        {
            var list = _orderRepo.GetAll(filters);

            return View(list);
        }

        [HttpGet]
        public IActionResult GenerateInvoice(int id)
        {
            Order order = _orderRepo.GetByIdWithProducts(id);

            InvoiceModel model = new()
			{
                InvoiceNumber = order.Id,
                OrderDate = order.OrderDateTime,
                Name = order.Name,
                Surname = order.Surname,
                PhoneNumber = order.PhoneNumber,
                Address = order.Address
            };



            List<OrderItem> items = new();

            foreach (var item in order.Products)
            {
                OrderItem orderItem = new()
                {
                    Price = item.Price,
                    Quantity = item.Quantity
                };

                var prod = _prodRepo.GetById(item.ProdId);
                var name = prod.Name + prod.Manufacturer;

                orderItem.Name = name;

                items.Add(orderItem);
            }

            model.Items = items;

			var document = new InvoiceDocument(model);

			byte[] pdfBytes = document.GeneratePdf();
			MemoryStream ms = new MemoryStream(pdfBytes);

            return File(ms, "application/octet-stream", $"Invoice #{id}.pdf");

		}

		[Authorize]
		public IActionResult Details(int id)
        {
            Order order = _orderRepo.GetByIdWithProducts(id);
            if(order == null)
            {
                return NotFound();
            }

            int price = _orderRepo.GetPriceById(id);

            OrderVM orderVM = new()
            {
                Id = order.Id,
                OrderDateTime = order.OrderDateTime,
                OrderNumber = order.OrderNumber,
                Address = order.Address,
                DeliveryType = order.DeliveryType,
                PaymentType = order.PaymentType,
                Name = order.Name,
                Surname = order.Surname,
                PhoneNumber = order.PhoneNumber,
                Status = order.Status
            };

			List<ProductsQuantityVM> products = new();

			foreach (var product in order.Products)
			{
				ProductsQuantityVM productsQuantity = new()
				{
					Quantity = product.Quantity,
					Prod = _prodRepo.GetById(product.ProdId)
				};
				products.Add(productsQuantity);
			}



            ViewBag.Price = price;

            orderVM.Products = products;

			return View(orderVM);
        }


		[Authorize]
        public IActionResult Create(List<CartVM> cart)
        {
            List<ProductsQuantityVM> products = new();

            foreach (var product in cart)
            {
                ProductsQuantityVM productsQuantity = new()
                {
                    Quantity = product.Quantity,
                    Prod = product.Product
                };
                products.Add(productsQuantity);
            }

			int price = products.Select(x => x.Prod.Price * x.Quantity).Sum();

			ViewBag.Price = price;

			return View(products);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (order.PhoneNumber != null && order.PhoneNumber.Length <10)
            {
                ModelState.AddModelError("", "Phone number is too short");
            }
            if (!ModelState.IsValid)
            {
				List<ProductsQuantityVM> products = new();

                foreach (var product in order.Products)
                {
                    ProductsQuantityVM productsQuantity = new()
                    {
                        Quantity = product.Quantity,
                        Prod = _prodRepo.GetById(product.ProdId)
                    };
                    products.Add(productsQuantity);
                }

				int price = products.Select(x => x.Prod.Price * x.Quantity).Sum();

				ViewBag.Price = price;

				return View(products);
            }

            foreach (var item in order.Products)
            {
                var prod = _prodRepo.GetByIdNoTracking(item.ProdId);
                prod.Left -= item.Quantity;
                _prodRepo.UpdateWithoutSave(prod);
            }

            var id = int.Parse(User.FindFirst("Id")?.Value);
			string key = cookiePrefix + id;
			var options = new CookieOptions
			{
				Expires = new DateTimeOffset(DateTime.Now.AddDays(-1d))
			};

			Response.Cookies.Append(key, "sfgdd", options);

			_orderRepo.Add(order);

            return RedirectToAction("Index", "Product");
		}

        [Authorize]
        public IActionResult Cancel(int id) 
        {
            var order = _orderRepo.GetById(id);
            order.Status = OrderStatus.Cancelled;
            _orderRepo.Update(order);
            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Update(int id, string orderNumber, OrderStatus orderStatus)
        {
            var order = _orderRepo.GetById(id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderNumber = orderNumber;
            order.Status = orderStatus;

            _orderRepo.Update(order);


            return RedirectToAction("List");
        }

        public IActionResult GenerateReport(OrderFilters filters)
        {
            var list = _orderRepo.GetAllWithProducts(filters);

            var model = new OrderReportModel()
            {
                StartDate = filters.MinDate,
                EndDate = filters.MaxDate,
                UkrPostCount = list.Where(o => o.DeliveryType == "Укрпошта").Count(),
                NovaPostCount = list.Where(o => o.DeliveryType == "Нова пошта").Count(),
                MeestCount = list.Where(o => o.DeliveryType == "Meest").Count(),
                CardCount = list.Where(o => o.PaymentType == "Credit card").Count(),
                CashCount = list.Where(o => o.PaymentType == "Cash").Count(),
                Items = list
			};

            var document = new OrderReportDocument(model);

			byte[] pdfBytes = document.GeneratePdf();
			MemoryStream ms = new(pdfBytes);

			return File(ms, "application/octet-stream", $"Order report from {filters.MinDate:d} to {filters.MaxDate:d}.pdf");
		}
	}
}
