using HouseholdOnlineStore.Interfaces;
using HouseholdOnlineStore.Models;
using HouseholdOnlineStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HouseholdOnlineStore.Controllers
{
    public class CartController : Controller
    {
        const string cookiePrefix = "cart_";
        private readonly IProductRepository _repo;
		readonly JsonSerializerOptions options1 = new()
		{
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
		};

		public CartController(IProductRepository repo)
        {
            _repo = repo;
        }

        [Authorize]
        public IActionResult Index()
        {
            var id = int.Parse(User.FindFirst("Id")?.Value);
            string key = cookiePrefix + id;
            var cartString = Request.Cookies[key];
            List<CartItem> cart = new();


			if (!string.IsNullOrEmpty(cartString))
            {
                cart = JsonSerializer.Deserialize<List<CartItem>>(cartString, options1);
            }

            List<CartVM> vm = new List<CartVM>();

            foreach (var item in cart)
            {
                vm.Add(
                    new CartVM
                    {
                        Product = _repo.GetByIdNoTracking(item.Product),
                        Quantity = item.Quantity
                    }
				);
			}

			int price = vm.Select(x => x.Product.Price * x.Quantity).Sum();

			ViewBag.Price = price;

			return View(vm);
        }

        [Authorize]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var prod = _repo.GetById(productId);
            if (prod == null)
            {
                return NotFound();
            }

            var id = int.Parse(User.FindFirst("Id")?.Value);
			string key = cookiePrefix + id;
			var cartString = Request.Cookies[key];
            List<CartItem> cart = new();

			if (!string.IsNullOrEmpty(cartString))
            {
                cart = JsonSerializer.Deserialize<List<CartItem>>(cartString, options1);
            }
            var item = new CartItem
            {
                Product = productId,
                Quantity = quantity
            };
            
            if(cart.Any(c => c.Product == productId))
            {
                var index = cart.FindIndex(c => c.Product ==  item.Product);
                cart[index].Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

			

			var cookieStr = JsonSerializer.Serialize(cart, options1);

            var options = new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.Now.AddDays(7)),
                
            };

			Response.Cookies.Append(key, cookieStr, options);

            return Redirect(Request.Headers["Referer"].ToString());

        }

        [Authorize]
        public IActionResult RemoveFromCart(int productId)
        {
            var id = int.Parse(User.FindFirst("Id")?.Value);
            string key = cookiePrefix + id;
            var cartString = Request.Cookies[key];
            List<CartItem> cart = new();

			if (!string.IsNullOrEmpty(cartString))
            {
                cart = JsonSerializer.Deserialize<List<CartItem>>(cartString, options1);
            }
            var item = cart.FirstOrDefault(c => c.Product == productId);
            cart.Remove(item);

			var cookieStr = JsonSerializer.Serialize(cart, options1);

            var options = new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.Now.AddDays(7))
            };

            Response.Cookies.Append(key, cookieStr, options);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize]
        public IActionResult Save(List<CartVM> cart)
        {
            List<CartItem> list = new();

            foreach (var item in cart)
            {
                CartItem item2 = new()
                {
                    Product = item.Product.Id,
                    Quantity = item.Quantity,
                };
                list.Add(item2);
            }

            var id = int.Parse(User.FindFirst("Id")?.Value);
			string key = cookiePrefix + id;

			var cookieStr = JsonSerializer.Serialize(list, options1);

			var options = new CookieOptions
			{
				Expires = new DateTimeOffset(DateTime.Now.AddDays(7))
			};

			Response.Cookies.Append(key, cookieStr, options);

			return Redirect(Request.Headers["Referer"].ToString());
		}
    }
}
