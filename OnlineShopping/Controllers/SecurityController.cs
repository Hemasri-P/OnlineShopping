using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Models;
using System.Security.Claims;

namespace OnlineShopping.Controllers
{
    public class SecurityController : Controller
    {
        OnlishopdbContext dc=new OnlishopdbContext();
        public IActionResult Home()
        {
            return View();

        }


        // Very secured and sensitive method
        [Authorize(Roles = "student")] 
        public IActionResult SecureMethod()
        {
            return View();
        }
        public IActionResult accessdeniedpage()
        {
            return View();
        }
        
        public IActionResult LogOut()
        {

            var Login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login( string txtuser, string txtpassword)
        {
            var res = (from t in dc.Registers
                      where t.Uname == txtuser && t.Password == txtpassword
                      select t).FirstOrDefault();
            if (res != null)
            {
                var userClaims = new List<Claim>()
             {
             new Claim(ClaimTypes.Name, res.Uname),
             new Claim(ClaimTypes.Role,res.Designation),
              new Claim(ClaimTypes.Country, res.Country),
             };
                var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction( "SecureMethod", "Security");
            }
            else
            {

                ViewData["msg"] = "Invalid username or password";
            }

            return View();
        }
       
    }
}
