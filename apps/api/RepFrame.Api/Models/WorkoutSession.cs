namespace RepFrame.Api.Models;

public class WorkoutSession
{
    public Guid Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public string? Note { get; set; }
    
    public ICollection<Set> Sets { get; set; } = [];
}
