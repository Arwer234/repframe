namespace RepFrame.Api.Models;

public class WorkoutSession
{
    public Guid Id { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
    
    public ICollection<Set> Sets { get; set; } = [];
}
