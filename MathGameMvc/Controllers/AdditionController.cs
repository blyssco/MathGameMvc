using MathGameMvc.Data;
using MathGameMvc.Models;
using MathGameMvc.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MathGameMvc.Controllers
{
    public class AdditionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHelperService _helperService;

        public AdditionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHelperService helperService)
        {
            _context = context;
            _userManager = userManager;
            _helperService = helperService;
        }
        private void SetViewData()
        {
            var numbers = _helperService.GetNumbers();
            int sum = numbers[0] + numbers[1];
            ViewBag.Number1 = numbers[0];
            ViewBag.Number2 = numbers[1];
            ViewBag.Sum = sum;
            ViewBag.CorrectAnswer = sum;
        }


        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public IActionResult Addition()
        {
            SetViewData();
            return View();
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Addition(int userAnswer, int correctAnswer)
        {
            SetViewData();

            int Score = HttpContext.Session.GetInt32("Score") ?? 0;
            int numberOfRounds = HttpContext.Session.GetInt32("NumberOfRounds") ?? 1;
            bool isCorrect = (userAnswer == correctAnswer);

            if (isCorrect)
            {
                ViewBag.Correct = true;
                Score++;
            }
            else
            {
                ViewBag.Correct = false;
            }
            numberOfRounds++;

            if (numberOfRounds < 6)
            {
                HttpContext.Session.SetInt32("NumberOfRounds", numberOfRounds);
                HttpContext.Session.SetInt32("Score", Score);
                return View();
            }
            else
            {
                SaveGame(Score);
                return RedirectToAction("Index", "Games");
            }
        }
        private void SaveGame(int score)
        {
            var user = _userManager.GetUserAsync(User).Result;
            Game game = new()
            {
                Date = DateTime.Now,
                Score = score,
                Type = GameType.Addition,
                UserId = user.Id
            };
            _context.Games.Add(game);
            _context.SaveChanges();
            HttpContext.Session.Remove("NumberOfRounds");
            HttpContext.Session.Remove("Score");
        }
    }
}


