﻿namespace Hoteis.Models.Entity;

public class Reservation : BaseClass
{
    public long ReservationId { get; set; }

    public string ClientUserId { get; set; } = null!;

    public ClientUser ClientUser { get; set; } = null!;

    public long RoomId { get; set; }

    public Room Room { get; set; } = null!;

    public DateTime CheckIn { get; set; }

    public DateTime CheckOut { get; set; }

    public decimal TotalPrice { get; set; }

    public ReservationStatus Status { get; set; } = null!;
}