using Demo2.Api.Models;

namespace Demo2.Api.Data;

public class LiftRepository
{
    private static readonly IReadOnlyList<Lift> Lifts =
    [
        new() { Id = 1, Name = "Skalet express", Status = "Open", WaitTimeMinutes = 15 },
        new() { Id = 2, Name = "Toppliften", Status = "Open", WaitTimeMinutes = 5 },
        new() { Id = 3, Name = "Hovde express", Status = "Open", WaitTimeMinutes = 10 },
        new() { Id = 4, Name = "Sydliften", Status = "Open", WaitTimeMinutes = 5 },
        new() { Id = 5, Name = "Väst express", Status = "Open", WaitTimeMinutes = 5 },
        new() { Id = 6, Name = "Pass express", Status = "Open", WaitTimeMinutes = 12 },
        new() { Id = 7, Name = "Mellanlliften", Status = "Open", WaitTimeMinutes = 0 }
    ];

    public Task<IEnumerable<Lift>> GetAllLifts()
    {
        return Task.FromResult(Lifts.AsEnumerable());
    }
}
