using SQLite;

namespace FitnessApp.Core.Models;

[Table("users")]
public class User
{
    [PrimaryKey]
    public string Id { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;
    
    public string Goal { get; set; } = string.Empty; // "Kraftaufbau", "Gewichtsverlust", etc.
    
    public string ExperienceLevel { get; set; } = "Anfänger"; // "Anfänger", "Fortgeschritten", "Experte"
    
    public string PreferredUnit { get; set; } = "kg"; // "kg" oder "lbs"
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsSynced { get; set; } = false;
}