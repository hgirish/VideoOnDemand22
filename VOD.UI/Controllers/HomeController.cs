using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using VOD.Common.Entities;
using VOD.Database.Services;
using VOD.UI.Models;

namespace VOD.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<VODUser> _signInManager;
       // private readonly IDbReadService _db;

        public HomeController(SignInManager<VODUser> signInManager
           // , IDbReadService db
            )
        {
            _signInManager = signInManager;
           // _db = db;
        }
        public IActionResult Index()
        {
            //_db.Include<Download>();
            //_db.Include<Module, Course>();
            //var result1 = await _db.SingleAsync<Download>(d => d.Id.Equals(3));
            //var result2 = await _db.GetAsync<Download>();
            //var result3 = await _db.GetAsync<Download>(d => d.ModuleId.Equals(1));
           // var result4 = await _db.AnyAsync<Download>(d => d.ModuleId.Equals(99));
           // var result5 = await _db.AnyAsync<Download>(d => d.ModuleId.Equals(1));

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
