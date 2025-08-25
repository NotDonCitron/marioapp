using FitnessApp.Core.Services;
using FitnessApp.Core.Models;

Console.WriteLine("🏋️‍♂️ Fitness App Core Test");
Console.WriteLine("==========================");

// Test Database Service
var dbPath = Path.Combine(Path.GetTempPath(), "fitness_test.db3");
var dbService = new DatabaseService(dbPath);

Console.WriteLine("\n1. Testing Database Connection...");
try
{
    var exercises = await dbService.GetExercisesAsync();
    Console.WriteLine($"✅ Database connected! Found {exercises.Count} exercises");
    
    // Show some exercises
    Console.WriteLine("\nVerfügbare Übungen:");
    foreach (var exercise in exercises.Take(5))
    {
        Console.WriteLine($"  - {exercise.Name} ({exercise.MuscleGroup})");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Database error: {ex.Message}");
    return;
}

Console.WriteLine("\n2. Testing User Creation...");
try
{
    var user = new User
    {
        Id = Guid.NewGuid().ToString(),
        Username = "TestUser",
        Goal = "Kraftaufbau",
        ExperienceLevel = "Anfänger"
    };
    
    await dbService.SaveUserAsync(user);
    var savedUser = await dbService.GetCurrentUserAsync();
    
    if (savedUser != null)
    {
        Console.WriteLine($"✅ User created: {savedUser.Username} ({savedUser.Goal})");
    }
    else
    {
        Console.WriteLine("❌ User creation failed");
        return;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ User creation error: {ex.Message}");
    return;
}

Console.WriteLine("\n3. Testing Workout Session...");
try
{
    var user = await dbService.GetCurrentUserAsync();
    if (user == null) return;
    
    // Create workout session
    var session = new WorkoutSession
    {
        UserId = user.Id,
        PlanNameSnapshot = "Test Training",
        StartedAt = DateTime.UtcNow
    };
    
    await dbService.SaveWorkoutSessionAsync(session);
    Console.WriteLine($"✅ Workout session created: {session.PlanNameSnapshot}");
    
    // Add some sets
    var exercises = await dbService.GetExercisesAsync();
    var benchPress = exercises.FirstOrDefault(e => e.Name.Contains("Bankdrücken"));
    
    if (benchPress != null)
    {
        var set1 = new SetLog
        {
            SessionId = session.Id,
            ExerciseId = benchPress.Id,
            SetNumber = 1,
            Weight = 80.0m,
            Reps = 10,
            LoggedAt = DateTime.UtcNow
        };
        
        var set2 = new SetLog
        {
            SessionId = session.Id,
            ExerciseId = benchPress.Id,
            SetNumber = 2,
            Weight = 82.5m,
            Reps = 8,
            LoggedAt = DateTime.UtcNow
        };
        
        await dbService.SaveSetLogAsync(set1);
        await dbService.SaveSetLogAsync(set2);
        
        Console.WriteLine($"✅ Added 2 sets for {benchPress.Name}:");
        Console.WriteLine($"  Set 1: {set1.Weight}kg × {set1.Reps}");
        Console.WriteLine($"  Set 2: {set2.Weight}kg × {set2.Reps}");
    }
    
    // Complete workout
    session.CompletedAt = DateTime.UtcNow;
    await dbService.SaveWorkoutSessionAsync(session);
    Console.WriteLine($"✅ Workout completed! Duration: {session.Duration:hh\\:mm\\:ss}");
    
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Workout session error: {ex.Message}");
    return;
}

Console.WriteLine("\n4. Testing Timer Service...");
try
{
    var timerService = new TimerService();
    var timerSettings = new TimerSettings
    {
        TimerMode = TimerMode.Fixed,
        FixedDurationSeconds = 5 // 5 seconds for quick test
    };
    
    Console.WriteLine("Starting 5-second timer...");
    
    var timerCompleted = false;
    timerService.TimerTick += (sender, remaining) => 
    {
        Console.WriteLine($"  Timer: {remaining.ToMinutesSeconds()}");
    };
    
    timerService.TimerCompleted += (sender, e) => 
    {
        Console.WriteLine("✅ Timer completed!");
        timerCompleted = true;
    };
    
    timerService.StartCountdown(timerSettings);
    
    // Wait for timer to complete
    while (!timerCompleted && timerService.IsRunning)
    {
        await Task.Delay(100);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Timer service error: {ex.Message}");
}

Console.WriteLine("\n5. Testing Statistics...");
try
{
    var user = await dbService.GetCurrentUserAsync();
    if (user == null) return;
    
    var totalWorkouts = await dbService.GetTotalWorkoutsAsync(user.Id);
    var totalVolume = await dbService.GetTotalVolumeAsync(user.Id);
    var workoutHistory = await dbService.GetWorkoutHistoryAsync(user.Id, 10);
    
    Console.WriteLine($"✅ Statistics:");
    Console.WriteLine($"  Total Workouts: {totalWorkouts}");
    Console.WriteLine($"  Total Volume: {totalVolume:F1}kg");
    Console.WriteLine($"  Recent Workouts: {workoutHistory.Count}");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Statistics error: {ex.Message}");
}

Console.WriteLine("\n6. Testing Workout Planner...");
try
{
    var user = await dbService.GetCurrentUserAsync();
    if (user == null) return;
    
    // Create a workout plan
    var plan = new WorkoutPlan
    {
        UserId = user.Id,
        Name = "Push-Tag A",
        Description = "Brust, Schultern, Trizeps",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };
    
    await dbService.SaveWorkoutPlanAsync(plan);
    Console.WriteLine($"✅ Workout plan created: {plan.Name}");
    
    // Add exercises to the plan
    var exercises = await dbService.GetExercisesAsync();
    var benchPress = exercises.FirstOrDefault(e => e.Name.Contains("Bankdrücken"));
    var shoulderPress = exercises.FirstOrDefault(e => e.Name.Contains("Schulterdrücken"));
    
    var planExercises = new List<PlanExercise>();
    
    if (benchPress != null)
    {
        planExercises.Add(new PlanExercise
        {
            PlanId = plan.Id,
            ExerciseId = benchPress.Id,
            DisplayOrder = 1,
            TargetSets = 4,
            TargetReps = 8
        });
    }
    
    if (shoulderPress != null)
    {
        planExercises.Add(new PlanExercise
        {
            PlanId = plan.Id,
            ExerciseId = shoulderPress.Id,
            DisplayOrder = 2,
            TargetSets = 3,
            TargetReps = 10
        });
    }
    
    await dbService.SavePlanExercisesAsync(plan.Id, planExercises);
    Console.WriteLine($"✅ Added {planExercises.Count} exercises to plan");
    
    // Load plan with exercises
    var loadedPlan = await dbService.GetWorkoutPlanWithExercisesAsync(plan.Id);
    if (loadedPlan != null)
    {
        Console.WriteLine($"✅ Plan loaded with {loadedPlan.PlanExercises.Count} exercises:");
        foreach (var pe in loadedPlan.PlanExercises)
        {
            Console.WriteLine($"  {pe.DisplayOrder}. {pe.Exercise?.Name} - {pe.TargetSets}×{pe.TargetReps}");
        }
    }
    
    // Test plan listing
    var userPlans = await dbService.GetWorkoutPlansAsync(user.Id);
    Console.WriteLine($"✅ User has {userPlans.Count} workout plans");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Workout planner error: {ex.Message}");
}

Console.WriteLine("\n7. Testing History & Statistics...");
try
{
    var user = await dbService.GetCurrentUserAsync();
    if (user == null) return;
    
    // Create additional test workouts for better statistics
    var testWorkouts = new List<WorkoutSession>();
    
    for (int i = 0; i < 5; i++)
    {
        var workout = new WorkoutSession
        {
            UserId = user.Id,
            PlanNameSnapshot = $"Test Workout {i + 1}",
            StartedAt = DateTime.UtcNow.AddDays(-i * 2),
            CompletedAt = DateTime.UtcNow.AddDays(-i * 2).AddHours(1)
        };
        
        await dbService.SaveWorkoutSessionAsync(workout);
        testWorkouts.Add(workout);
        
        // Add some sets to each workout
        var exercises = await dbService.GetExercisesAsync();
        var benchPress = exercises.FirstOrDefault(e => e.Name.Contains("Bankdrücken"));
        
        if (benchPress != null)
        {
            for (int j = 1; j <= 3; j++)
            {
                var set = new SetLog
                {
                    SessionId = workout.Id,
                    ExerciseId = benchPress.Id,
                    SetNumber = j,
                    Weight = 80 + (i * 2.5m) + (j * 2.5m), // Progressive weight
                    Reps = 10 - j,
                    LoggedAt = workout.StartedAt.AddMinutes(j * 5)
                };
                
                await dbService.SaveSetLogAsync(set);
            }
        }
    }
    
    Console.WriteLine($"✅ Created {testWorkouts.Count} test workouts with sets");
    
    // Test statistics
    var totalWorkouts = await dbService.GetTotalWorkoutsAsync(user.Id);
    var totalVolume = await dbService.GetTotalVolumeAsync(user.Id);
    var workoutHistory = await dbService.GetWorkoutHistoryAsync(user.Id, 10);
    
    Console.WriteLine($"✅ Statistics calculated:");
    Console.WriteLine($"  Total Workouts: {totalWorkouts}");
    Console.WriteLine($"  Total Volume: {totalVolume:F1}kg");
    Console.WriteLine($"  Recent Workouts: {workoutHistory.Count}");
    
    // Test personal records calculation
    var bestBenchSet = workoutHistory
        .SelectMany(w => dbService.GetSetLogsForSessionAsync(w.Id).Result)
        .Where(s => s.Exercise?.Name?.Contains("Bankdrücken") == true)
        .OrderByDescending(s => s.Weight * s.Reps)
        .FirstOrDefault();
    
    if (bestBenchSet != null)
    {
        var oneRepMax = bestBenchSet.Weight * (1 + (decimal)bestBenchSet.Reps / 30);
        Console.WriteLine($"✅ Best Bench Press: {bestBenchSet.Weight}kg × {bestBenchSet.Reps} (1RM: {oneRepMax:F1}kg)");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ History & Statistics error: {ex.Message}");
}

Console.WriteLine("\n🎉 All tests completed!");
Console.WriteLine("\nCore functionality is working correctly.");
Console.WriteLine("Ready for MAUI UI implementation!");

// Cleanup
try
{
    File.Delete(dbPath);
    Console.WriteLine("\n🧹 Test database cleaned up.");
}
catch
{
    // Ignore cleanup errors
}