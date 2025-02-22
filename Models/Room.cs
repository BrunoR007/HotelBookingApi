namespace Hoteis.Models;

public class Room
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;
    public string RoomNumber { get; set; } = null!; // Ex.: "101", "A-12"
    public string Type { get; set; } = null!; // Ex.: "Standard", "Suite"
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; } // Capacidade máxima (ex.: 2 pessoas)
    public string Description { get; set; } = null!; // Informações adicionais
    public List<RoomPhoto> Photos { get; set; } = []; // Fotos do quarto
    public List<Reservation> Reservations { get; set; } = []; // Reservas associadas
}

// RoomPhoto.cs (Nova entidade para fotos)
public class RoomPhoto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public string Url { get; set; } = null!; // URL da foto (armazenada em servidor ou cloud)
    public string Description { get; set; } = null!; // Descrição da foto (ex.: "Vista da janela")
}
