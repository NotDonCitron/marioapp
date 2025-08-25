using FitnessApp.Core.Models;
using FitnessApp.Core.Services;

Console.WriteLine("🏋️‍♂️ Fitness App - Console Demo");
Console.WriteLine("================================");

// Initialize database
var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fitness_test.db");
var databaseService = new DatabaseService(databasePath);

Console.WriteLine("✅ Database initialized successfully!");

// Get exercises
var exercises = await databaseService.GetExercisesAsync();
Console.WriteLine($"📋 Available exercises: {exercises.Count}");

foreach (var exercise in exercises.Take(5))
{
    Console.WriteLine($"   • {exercise.Name} ({exercise.MuscleGroup})");
}

// Create a test user
var user = new User
{
    Id = Guid.NewGuid().ToString(),
    Username = "Test User",
    Goal = "Kraftaufbau",
    CreatedAt = DateTime.UtcNow
};

await databaseService.SaveUserAsync(user);
Console.WriteLine($"👤 Created user: {user.Username}");

// Start a workout session
var workoutSession = new WorkoutSession
{
    Id = Guid.NewGuid().ToString(),
    UserId = user.Id,
    PlanNameSnapshot = "Test Workout",
    StartedAt = DateTime.UtcNow
};

await databaseService.SaveWorkoutSessionAsync(workoutSession);
Console.WriteLine($"🏃‍♂️ Started workout session: {workoutSession.PlanNameSnapshot}");

// Log some sets
var benchPress = exercises.First(e => e.Name.Contains("Bankdrücken"));
var sets = new List<SetLog>
{
    new SetLog
    {
        SessionId = workoutSession.Id,
        ExerciseId = benchPress.Id,
        SetNumber = 1,
        Weight = 60,
        Reps = 10,
        LoggedAt = DateTime.UtcNow
    },
    new SetLog
    {
        SessionId = workoutSession.Id,
        ExerciseId = benchPress.Id,
        SetNumber = 2,
        Weight = 65,
        Reps = 8,
        LoggedAt = DateTime.UtcNow.AddMinutes(3)
    },
    new SetLog
    {
        SessionId = workoutSession.Id,
        ExerciseId = benchPress.Id,
        SetNumber = 3,
        Weight = 70,
        Reps = 6,
        LoggedAt = DateTime.UtcNow.AddMinutes(6)
    }
};

foreach (var set in sets)
{
    await databaseService.SaveSetLogAsync(set);
    Console.WriteLine($"💪 Logged set {set.SetNumber}: {set.Weight}kg × {set.Reps} reps");
}

// Complete the workout
workoutSession.CompletedAt = DateTime.UtcNow.AddMinutes(30);
await databaseService.SaveWorkoutSessionAsync(workoutSession);

Console.WriteLine($"✅ Completed workout! Duration: {workoutSession.Duration:hh\\:mm\\:ss}");

// Get statistics
var totalWorkoutsCount = await databaseService.GetTotalWorkoutsAsync(user.Id);
var totalVolume = sets.Sum(s => (double)(s.Weight * s.Reps));

Console.WriteLine();
Console.WriteLine("📊 Statistics:");
Console.WriteLine($"   • Total workouts: {totalWorkoutsCount}");
Console.WriteLine($"   • Total volume: {totalVolume}kg");
Console.WriteLine($"   • Best set: {sets.OrderByDescending(s => s.Weight).First().Weight}kg × {sets.OrderByDescending(s => s.Weight).First().Reps}");

// Calculate 1RM using Epley formula
var bestSet = sets.OrderByDescending(s => s.Weight * s.Reps).First();
var oneRepMax = (double)bestSet.Weight * (1 + (double)bestSet.Reps / 30.0);
Console.WriteLine($"   • Estimated 1RM (Bankdrücken): {oneRepMax:F1}kg");

Console.WriteLine();
Console.WriteLine("🎉 Demo completed successfully!");
Console.WriteLine("The fitness app core functionality is working perfectly!");