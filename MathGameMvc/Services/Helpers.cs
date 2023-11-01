using MathGameMvc.Data;
using MathGameMvc.Models;
using Microsoft.AspNetCore.Identity;


namespace Helpers;

public class Helpers : IHelperService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly HttpContext _httpContext;
    private readonly ApplicationDbContext _context;

    public Helpers(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
    {
        _userManager = userManager;
        _httpContext = httpContextAccessor.HttpContext;
        _context = context;
    }
    public async Task<string> GetNameAsync()
    {
        var user = await _userManager.GetUserAsync(_httpContext.User);

        if (user != null)
        {
            var gamerTag = user.GamerTag;
            return gamerTag;
        }

        return null;
    }
    public int[] GetNumbers()
    {
        var random = new Random();
        var firstNumber = random.Next(1, 100);
        var secondNumber = random.Next(1, 100);

        var result = new int[2];
        result[0] = firstNumber;
        result[1] = secondNumber;

        return result;
    }

}