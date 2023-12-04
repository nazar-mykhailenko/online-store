using HouseholdOnlineStore.Data;
using HouseholdOnlineStore.Interfaces;
using HouseholdOnlineStore.Models;
using HouseholdOnlineStore.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace HouseholdOnlineStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _repo;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Index(string search)
        {
            var users = _repo.GetUsers(search);
            var admins = _repo.GetAdmins(search);

            return View((admins, users));
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel userData)
        {
            AppUser? user = _repo.GetByEmail(userData.Email);

			if (!Regex.IsMatch(userData.Email, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"))
			{
				ModelState.AddModelError("Email", "Wrong email format");
			}
			else if(user == null)
            {
				ModelState.AddModelError("Email", "Cannot find user with this email");
			}            
			else if (user.Password != userData.Password)
            {
				ModelState.AddModelError("Password", "Wrong password");
			}
            if (ModelState.IsValid)
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };
                if (user.Name != null)
                    claims.Add(new Claim(ClaimTypes.Name, user.Name));

				if (user.Surname != null)
					claims.Add(new Claim(ClaimTypes.Surname, user.Surname));


				var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = userData.RememberMe
                };


                await HttpContext.SignInAsync("CookieAuth", principal,authProperties);
                
                return RedirectToAction("Index", "Product");
            }

            return View(userData);
        }
        [Authorize]
		public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Product");
        }

        AppUser ToEntity(RegistrationModel model)
        {
            AppUser user = new()
            {
                Email = model.Email,
                Password = model.Password,
                Name = model.Name,
                Surname = model.Surname,
                Role = "User"
            };
            return user;
        }
        
		public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegistrationModel userData)
        {
			if (!Regex.IsMatch(userData.Email, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"))
			{
				ModelState.AddModelError("Email", "Wrong email format");
			}
			else if (_repo.CheckIfUserExistsByEmail(userData.Email))
            {
                ModelState.AddModelError("Email", "User with this email already exists");
            }
            else if(userData.Password.Length < 6)
            {
				ModelState.AddModelError("Password", "Password is too short");
			}
            else if(userData.Password != userData.ConfirmPassword)
            {
				ModelState.AddModelError("ConfirmPassword", "Passwords must be the same");
			}
			if (ModelState.IsValid)
            {
                AppUser user = ToEntity(userData);
                _repo.Add(user);

                return RedirectToAction("Index", "Product");
            }

            return View(userData);
        }
        
        public IActionResult AccessDenied() 
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Control()
        {
            return View();
        }
        [Authorize]
        public IActionResult Settings(int id)
        {
            var user = _repo.GetById(id);
            if (!User.HasClaim("Id", id.ToString()))
            {
                return AccessDenied();
            }

            var VM = new EditUserVM
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname
            };

            return View(VM);
        }

        [HttpPost] 
        public IActionResult Settings(EditUserVM VM)
        {
            var user = _repo.GetById(VM.Id);

            if(!string.IsNullOrEmpty(VM.OldPassword) && !string.IsNullOrEmpty(VM.NewPassword))
            {
                if (VM.OldPassword != user.Password)
                {
                    ModelState.AddModelError("OldPassword", "Old Password doesn`t match");
                }
                else if(VM.NewPassword != VM.ConfirmPassword) 
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords must be the same");
                }
            }
            else
            {
                VM.NewPassword = user.Password;
            }

            if (ModelState.IsValid)
            {
                user.Name = VM.Name;
                user.Surname = VM.Surname;
                user.Email = VM.Email;
                user.Password = VM.NewPassword;

                _repo.Update(user);
                return RedirectToAction("Index", "Product");
            }

            return View(VM);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(EditUserVM VM)
        {
            var user = _repo.GetById(VM.Id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Id == int.Parse(User.FindFirstValue("Id")))
            {
                await HttpContext.SignOutAsync("CookieAuth");
				_repo.Delete(user);
				return RedirectToAction("Index", "Product");
			}

			_repo.Delete(user);
			return RedirectToAction("Index", "User");
		}

        [Authorize(Roles ="Admin")]
        public IActionResult AddAdmin(int id)
        {
            var user = _repo.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Role = "Admin";

            _repo.Update(user);

            return RedirectToAction("Index", "User");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAdmin(int id)
        {
            var user = _repo.GetById(id);

            if (user == null)
            {
                return NotFound();
            }
            user.Role = "User";
            _repo.Update(user);

            return RedirectToAction("Index", "User");

        }
    }
}
