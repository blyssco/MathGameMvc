using Microsoft.AspNetCore.Identity;

namespace MathGameMvc.Models;

public class ApplicationUser : IdentityUser
{
    public string GamerTag { get; set; }
    public ICollection<Game> Games { get; set; } = new List<Game>();
}
