﻿namespace Hoteis.Models.Entity;

public class BaseClass
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ModifiedAt { get; set; }
}
