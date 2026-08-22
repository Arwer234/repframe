namespace RepFrame.Api.Models;

public class Set
{
    public Guid Id { get; set; }
    public Guid WorkoutSessionId { get; set; }
    public Guid ExerciseId { get; set; }
    public int Number { get; set; }
    public decimal WeightKg { get; set; }
    public int Reps { get; set; }
    public int? Rir { get; set; } // Reps In Reserve
    public SetType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public WorkoutSession WorkoutSession { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
}

public enum SetType
{
    Working = 0,
    WarmUp = 1,
    TopSet = 2,
    BackOff = 3
}
