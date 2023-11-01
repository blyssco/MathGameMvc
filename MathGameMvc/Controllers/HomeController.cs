using MathGameMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MathGameMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHelperService _helperService;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, IHelperService helperService)
        {
            _logger = logger;
            _userManager = userManager;
            _helperService = helperService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var gamerTag = await _helperService.GetNameAsync();

            ViewData["GamerTag"] = gamerTag;

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}