namespace Demo1.Api.Models;

public class Lift
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }
    public int WaitTimeMinutes { get; set; }
}
