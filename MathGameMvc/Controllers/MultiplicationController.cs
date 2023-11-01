using MathGameMvc.Data;
using MathGameMvc.Models;
using MathGameMvc.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MathGameMvc.Controllers
{
    public class MultiplicationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MultiplicationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<IActionResult> Multiplication()
        {
            Random random = new Random();
            int firstNumber = random.Next(0, 10);
            int secondNumber = random.Next(0, 10);
            int sum = firstNumber * secondNumber;
            ViewBag.Number1 = firstNumber;
            ViewBag.Number2 = secondNumber;
            ViewBag.Sum = sum;

            return View();
        }
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Multiplication(int userAnswer, int correctAnswer)
        {
            var user = await _userManager.GetUserAsync(User);

            Random random = new Random();
            int firstNumber = random.Next(0, 10);
            int secondNumber = random.Next(0, 10);
            int sum = firstNumber * secondNumber;
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
                    Type = GameType.Multiplication,
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

