namespace Hoteis.Models.Entity;

public class ReservationStatus : BaseClass
{
    public int ReservationStatusId { get; set; }

    public required string Name { get; set; }
}
