namespace Hoteis.Models;

public class State : BaseClass
{
    public long StateId { get; set; }

    public string Code { get; set; } = null!; 
    
    public string Name { get; set; } = null!; 
    
    public List<City> Cities { get; set; } = [];
}
