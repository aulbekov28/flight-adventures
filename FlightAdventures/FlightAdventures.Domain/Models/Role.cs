namespace FlightAdventures.Domain.Models;

public class Role
{
    public int Id { get; set; }
    public string Code { get; set; }
    
    public virtual ICollection<User> Users { get; set; }
}