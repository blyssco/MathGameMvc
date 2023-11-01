using MathGameMvc.Data;
using MathGameMvc.Models;
using MathGameMvc.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MathGameMvc.Controllers
{
    public class DivisionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DivisionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<IActionResult> Division()
        {
            Random random = new Random();
            int firstNumber = random.Next(0, 100);
            int secondNumber = random.Next(0, 100);
            while (firstNumber % secondNumber != 0)
            {
                firstNumber = random.Next(1, 100);
                secondNumber = random.Next(1, 100);
            }
            int sum = firstNumber / secondNumber;
            ViewBag.Number1 = firstNumber;
            ViewBag.Number2 = secondNumber;
            ViewBag.Sum = sum;

            return View();
        }
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Division(int userAnswer, int correctAnswer)
        {
            var user = await _userManager.GetUserAsync(User);

            Random random = new Random();
            int firstNumber = random.Next(0, 100);
            int secondNumber = random.Next(0, 100);
            while (firstNumber % secondNumber != 0)
            {
                firstNumber = random.Next(1, 99);
                secondNumber = random.Next(1, 99);
            }
            int sum = firstNumber / secondNumber;
            ViewBag.Number1 = firstNumber;
            ViewBag.Number2 = secondNumber;
            ViewBag.Sum = sum;
            ViewBag.CorrectAnswer = correctAnswer;

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
                Game game = new()
                {
                    Date = DateTime.Now,
                    Score = Score,
                    Type = GameType.Division,
                    UserId = user.Id
                };
                _context.Games.Add(game);
                _context.SaveChanges();
                HttpContext.Session.Remove("NumberOfRounds");
                HttpContext.Session.Remove("Score");

                return RedirectToAction("Index", "Games");
            }

        }
    }
}

