using SQLite;

namespace FitnessApp.Core.Models;

[Table("exercises")]
public class Exercise
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    [Unique]
    public string Name { get; set; } = string.Empty;
    
    public string MuscleGroup { get; set; } = string.Empty; // "Brust", "Beine", "Rücken", etc.
    
    public string Description { get; set; } = string.Empty;
    
    public bool IsCustom { get; set; } = false; // User-created vs. predefined
    
    public string UserId { get; set; } = string.Empty; // Only for custom exercises
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsSynced { get; set; } = false;
}