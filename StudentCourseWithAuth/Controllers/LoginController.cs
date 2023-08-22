using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StudentCourseWithAuth.Logger;
using StudentCourseWithAuth.Models;
using System.Net;
using System.Security.Claims;

namespace StudentCourseWithAuth.Controllers
{
    public class LoginController : Controller
    {
        //public VMLogin Credential { get; set; }
        private readonly IMyLogger _logger;

        public LoginController(IMyLogger logger) {
            _logger = logger;
        }

        public IActionResult LogInAction() {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Menu/Index");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync("MyCookieAuth");
                _logger.LogInfo($"{User.Identity.Name} logged out");
                return Redirect("/Home/Index"); // Redirect to a desired page after logout
            }
            catch {
                _logger.LogError($"{User.Identity.Name} log out failed");
                return Redirect("/Home/Index");
            }
            
        }


        public IActionResult InvalidLogin() {
            return View();
        }

        public IActionResult NotAuthorized() { return View(); }

        [HttpPost]
        public async Task<IActionResult> LogIn(VMLogin Credential)
        {

            if (!ModelState.IsValid) return View();

            // Verify the credential
            
            if (Credential.UserName == "admin" && Credential.Password == "password")
            {
                // Creating the security context
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", "2021-02-01"),
                    new Claim("Power", "10")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };
                //await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                try{
                    await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
                    _logger.LogInfo($"{Credential.UserName} Logged in successfully");
                    return Redirect("/Menu/Index");
                }
                catch{
                    _logger.LogError($"{Credential.UserName} Log in failed");
                }
            }
            else if (Credential.UserName == "teacher" && Credential.Password == "password") {

                // creating a new security context for teacher
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "teacher"),
                    new Claim(ClaimTypes.Email, "teacher@mywebsite.com"),
                    new Claim("Department", "Teacher"),
                    //new Claim("Admin", "false"),
                    new Claim("Manager", "false"),
                    new Claim("EmploymentDate", "2023-08-01"),
                    new Claim("Power", "6")

                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };
                try
                {
                    await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
                    _logger.LogInfo($"{Credential.UserName} Logged in successfully");
                    return Redirect("/Menu/Index");
                }
                catch {
                    _logger.LogError($"{Credential.UserName} Log in failed");
                }
                

            }

            
            return Redirect("/Login/InvalidLogin");
        }
    }
}
