using Demo2.Api.Models;

namespace Demo2.Api.Data;

public class LiftRepository
{
    private static readonly IReadOnlyList<Lift> Lifts =
    [
        new() { Id = 1, Name = "Gondola A", Status = "Open", WaitTimeMinutes = 15 },
        new() { Id = 2, Name = "T-Bar 1", Status = "Open", WaitTimeMinutes = 5 },
        new() { Id = 3, Name = "Chairlift North", Status = "Open", WaitTimeMinutes = 25 },
        new() { Id = 4, Name = "Chairlift South", Status = "Closed", WaitTimeMinutes = 0 },
        new() { Id = 5, Name = "Express Lift East", Status = "Open", WaitTimeMinutes = 12 },
        new() { Id = 6, Name = "Surface Lift West", Status = "Maintenance", WaitTimeMinutes = 0 },
        new() { Id = 7, Name = "Alpine Peak Gondola", Status = "Open", WaitTimeMinutes = 30 }
    ];

    public Task<IEnumerable<Lift>> GetAllLifts()
    {
        return Task.FromResult(Lifts.AsEnumerable());
    }
}
