using EmployeeMangementSystem.Data;
using EmployeeMangementSystem.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeMangementSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AppDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Login page");
                return StatusCode(500, "Unable to load login page.");
            }
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var user = await _context.Users
                    .FirstOrDefaultAsync(u =>
                        u.UserName == model.UserName &&
                        u.Password == model.Password);

                if (user == null)
                {
                    ViewBag.Error = "Invalid Username or Password";
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                // Redirect back to originally requested page
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Login");
                ViewBag.Error = "An unexpected error occurred. Please try again.";
                return View(model);
            }
        }

        // Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Logout");
                return StatusCode(500, "Logout failed.");
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
