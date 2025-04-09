namespace Hoteis.Models.Entity;

public class City : BaseClass
{
    public long CityId { get; set; }

    public long StateId { get; set; }

    public State State { get; set; } = null!;

    public string Name { get; set; } = null!;
}
