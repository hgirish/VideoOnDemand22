using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using VOD.Common.Entities;
using VOD.UI.Models;

namespace VOD.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<VODUser> _signInManager;


        public HomeController(SignInManager<VODUser> signInManager
            )
        {
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/Account/Login", new { Area = "Identity" });
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
