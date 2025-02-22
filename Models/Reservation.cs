namespace Hoteis.Models;

public class Reservation
{
    public long Id { get; set; }
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    public long RoomId { get; set; } // Agora vinculada a um quarto, não ao hotel diretamente
    public Room Room { get; set; } = null!;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal TotalPrice { get; set; }
}