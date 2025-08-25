using SQLite;

namespace FitnessApp.Core.Models;

[Table("workout_sessions")]
public class WorkoutSession
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string UserId { get; set; } = string.Empty;
    
    public string? PlanId { get; set; } // Null for free workouts
    
    public string PlanNameSnapshot { get; set; } = string.Empty; // Name at time of workout
    
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? CompletedAt { get; set; }
    
    public string Notes { get; set; } = string.Empty;
    
    public bool IsSynced { get; set; } = false;
    
    // Navigation properties
    [Ignore]
    public List<SetLog> SetLogs { get; set; } = new();
    
    [Ignore]
    public TimeSpan Duration => CompletedAt.HasValue 
        ? CompletedAt.Value - StartedAt 
        : DateTime.UtcNow - StartedAt;
    
    [Ignore]
    public bool IsCompleted => CompletedAt.HasValue;
}

[Table("set_logs")]
public class SetLog
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string SessionId { get; set; } = string.Empty;
    
    public int ExerciseId { get; set; }
    
    public int SetNumber { get; set; }
    
    public decimal Weight { get; set; }
    
    public int Reps { get; set; }
    
    public int RestTimeSeconds { get; set; } = 0;
    
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsSynced { get; set; } = false;
    
    // Navigation property
    [Ignore]
    public Exercise? Exercise { get; set; }
}