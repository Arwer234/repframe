namespace RepFrame.Api.Models;

public class Exercise
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? MuscleGroup { get; set; }
    
    public ICollection<Set> Sets { get; set; } = [];
}
