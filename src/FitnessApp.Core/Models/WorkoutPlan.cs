using SQLite;

namespace FitnessApp.Core.Models;

[Table("workout_plans")]
public class WorkoutPlan
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string UserId { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsSynced { get; set; } = false;
    
    // Navigation property (not stored in DB)
    [Ignore]
    public List<PlanExercise> PlanExercises { get; set; } = new();
}

[Table("plan_exercises")]
public class PlanExercise
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string PlanId { get; set; } = string.Empty;
    
    public int ExerciseId { get; set; }
    
    public int DisplayOrder { get; set; }
    
    public int TargetSets { get; set; } = 3;
    
    public int TargetReps { get; set; } = 10;
    
    public decimal TargetWeight { get; set; } = 0;
    
    public bool IsSynced { get; set; } = false;
    
    // Navigation property
    [Ignore]
    public Exercise? Exercise { get; set; }
}