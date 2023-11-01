using MathGameMvc.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace MathGameMvc.Models;

public class Game
{
    [Key]
    public int GameId { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public GameType Type { get; set; }


    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
