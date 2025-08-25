using SQLite;
using FitnessApp.Core.Models;

namespace FitnessApp.Core.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _databasePath;

    public DatabaseService(string databasePath)
    {
        _databasePath = databasePath;
    }

    private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_database is not null)
            return _database;

        _database = new SQLiteAsyncConnection(_databasePath);
        
        // Create tables
        await _database.CreateTableAsync<User>();
        await _database.CreateTableAsync<Exercise>();
        await _database.CreateTableAsync<WorkoutPlan>();
        await _database.CreateTableAsync<PlanExercise>();
        await _database.CreateTableAsync<WorkoutSession>();
        await _database.CreateTableAsync<SetLog>();
        
        // Seed default exercises if empty
        await SeedDefaultExercisesAsync();
        
        return _database;
    }

    #region User Operations
    public async Task<User?> GetCurrentUserAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<User>().FirstOrDefaultAsync();
    }

    public async Task<int> SaveUserAsync(User user)
    {
        var db = await GetDatabaseAsync();
        user.UpdatedAt = DateTime.UtcNow;
        user.IsSynced = false;
        
        if (await db.Table<User>().Where(u => u.Id == user.Id).CountAsync() > 0)
            return await db.UpdateAsync(user);
        else
            return await db.InsertAsync(user);
    }
    #endregion

    #region Exercise Operations
    public async Task<List<Exercise>> GetExercisesAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<Exercise>().OrderBy(e => e.MuscleGroup).ThenBy(e => e.Name).ToListAsync();
    }

    public async Task<List<Exercise>> GetExercisesByMuscleGroupAsync(string muscleGroup)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<Exercise>()
            .Where(e => e.MuscleGroup == muscleGroup)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<int> SaveExerciseAsync(Exercise exercise)
    {
        var db = await GetDatabaseAsync();
        exercise.IsSynced = false;
        
        if (exercise.Id == 0)
            return await db.InsertAsync(exercise);
        else
            return await db.UpdateAsync(exercise);
    }
    #endregion

    #region Workout Plan Operations
    public async Task<List<WorkoutPlan>> GetWorkoutPlansAsync(string userId)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<WorkoutPlan>()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.UpdatedAt)
            .ToListAsync();
    }

    public async Task<WorkoutPlan?> GetWorkoutPlanWithExercisesAsync(string planId)
    {
        var db = await GetDatabaseAsync();
        var plan = await db.Table<WorkoutPlan>().Where(p => p.Id == planId).FirstOrDefaultAsync();
        
        if (plan == null) return null;

        var planExercises = await db.Table<PlanExercise>()
            .Where(pe => pe.PlanId == planId)
            .OrderBy(pe => pe.DisplayOrder)
            .ToListAsync();

        foreach (var planExercise in planExercises)
        {
            planExercise.Exercise = await db.Table<Exercise>()
                .Where(e => e.Id == planExercise.ExerciseId)
                .FirstOrDefaultAsync();
        }

        plan.PlanExercises = planExercises;
        return plan;
    }

    public async Task<int> SaveWorkoutPlanAsync(WorkoutPlan plan)
    {
        var db = await GetDatabaseAsync();
        plan.UpdatedAt = DateTime.UtcNow;
        plan.IsSynced = false;
        
        var existingPlan = await db.Table<WorkoutPlan>().Where(p => p.Id == plan.Id).FirstOrDefaultAsync();
        
        if (existingPlan == null)
            return await db.InsertAsync(plan);
        else
            return await db.UpdateAsync(plan);
    }

    public async Task SavePlanExercisesAsync(string planId, List<PlanExercise> exercises)
    {
        var db = await GetDatabaseAsync();
        
        // Delete existing plan exercises
        await db.Table<PlanExercise>().Where(pe => pe.PlanId == planId).DeleteAsync();
        
        // Insert new ones
        foreach (var exercise in exercises)
        {
            exercise.PlanId = planId;
            exercise.IsSynced = false;
            await db.InsertAsync(exercise);
        }
    }

    public async Task<int> DeleteWorkoutPlanAsync(string planId)
    {
        var db = await GetDatabaseAsync();
        
        // Delete plan exercises first
        await db.Table<PlanExercise>().Where(pe => pe.PlanId == planId).DeleteAsync();
        
        // Delete the plan
        return await db.Table<WorkoutPlan>().Where(p => p.Id == planId).DeleteAsync();
    }
    #endregion

    #region Workout Session Operations
    public async Task<WorkoutSession?> GetActiveWorkoutSessionAsync(string userId)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<WorkoutSession>()
            .Where(s => s.UserId == userId && s.CompletedAt == null)
            .FirstOrDefaultAsync();
    }

    public async Task<List<WorkoutSession>> GetWorkoutHistoryAsync(string userId, int limit = 50)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<WorkoutSession>()
            .Where(s => s.UserId == userId && s.CompletedAt != null)
            .OrderByDescending(s => s.StartedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<int> SaveWorkoutSessionAsync(WorkoutSession session)
    {
        var db = await GetDatabaseAsync();
        session.IsSynced = false;
        
        var existing = await db.Table<WorkoutSession>().Where(s => s.Id == session.Id).FirstOrDefaultAsync();
        
        if (existing == null)
            return await db.InsertAsync(session);
        else
            return await db.UpdateAsync(session);
    }

    public async Task<int> SaveSetLogAsync(SetLog setLog)
    {
        var db = await GetDatabaseAsync();
        setLog.IsSynced = false;
        
        if (setLog.Id == 0)
            return await db.InsertAsync(setLog);
        else
            return await db.UpdateAsync(setLog);
    }

    public async Task<List<SetLog>> GetSetLogsForSessionAsync(string sessionId)
    {
        var db = await GetDatabaseAsync();
        var setLogs = await db.Table<SetLog>()
            .Where(sl => sl.SessionId == sessionId)
            .OrderBy(sl => sl.ExerciseId)
            .ThenBy(sl => sl.SetNumber)
            .ToListAsync();

        // Load exercise details
        foreach (var setLog in setLogs)
        {
            setLog.Exercise = await db.Table<Exercise>()
                .Where(e => e.Id == setLog.ExerciseId)
                .FirstOrDefaultAsync();
        }

        return setLogs;
    }
    #endregion

    #region Statistics
    public async Task<SetLog?> GetLastSetForExerciseAsync(string userId, int exerciseId)
    {
        var db = await GetDatabaseAsync();
        
        var lastSession = await db.Table<WorkoutSession>()
            .Where(s => s.UserId == userId && s.CompletedAt != null)
            .OrderByDescending(s => s.CompletedAt)
            .FirstOrDefaultAsync();

        if (lastSession == null) return null;

        return await db.Table<SetLog>()
            .Where(sl => sl.SessionId == lastSession.Id && sl.ExerciseId == exerciseId)
            .OrderByDescending(sl => sl.SetNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetTotalWorkoutsAsync(string userId)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<WorkoutSession>()
            .Where(s => s.UserId == userId && s.CompletedAt != null)
            .CountAsync();
    }

    public async Task<decimal> GetTotalVolumeAsync(string userId)
    {
        var db = await GetDatabaseAsync();
        var sessions = await db.Table<WorkoutSession>()
            .Where(s => s.UserId == userId && s.CompletedAt != null)
            .ToListAsync();

        decimal totalVolume = 0;
        foreach (var session in sessions)
        {
            var sets = await db.Table<SetLog>()
                .Where(sl => sl.SessionId == session.Id)
                .ToListAsync();
            
            totalVolume += sets.Sum(s => s.Weight * s.Reps);
        }

        return totalVolume;
    }
    #endregion

    private async Task SeedDefaultExercisesAsync()
    {
        var db = await GetDatabaseAsync();
        var count = await db.Table<Exercise>().CountAsync();
        
        if (count > 0) return; // Already seeded

        var defaultExercises = new List<Exercise>
        {
            // Brust
            new() { Name = "Bankdrücken", MuscleGroup = "Brust", Description = "Klassisches Bankdrücken mit Langhantel" },
            new() { Name = "Kurzhantel Bankdrücken", MuscleGroup = "Brust", Description = "Bankdrücken mit Kurzhanteln" },
            new() { Name = "Liegestütze", MuscleGroup = "Brust", Description = "Klassische Liegestütze" },
            
            // Beine
            new() { Name = "Kniebeugen", MuscleGroup = "Beine", Description = "Tiefe Kniebeugen mit Langhantel" },
            new() { Name = "Beinpresse", MuscleGroup = "Beine", Description = "Beinpresse an der Maschine" },
            new() { Name = "Ausfallschritte", MuscleGroup = "Beine", Description = "Ausfallschritte mit oder ohne Gewicht" },
            
            // Rücken
            new() { Name = "Kreuzheben", MuscleGroup = "Rücken", Description = "Klassisches Kreuzheben" },
            new() { Name = "Klimmzüge", MuscleGroup = "Rücken", Description = "Klimmzüge am Reck" },
            new() { Name = "Rudern", MuscleGroup = "Rücken", Description = "Rudern mit Langhantel oder Kabel" },
            
            // Schultern
            new() { Name = "Schulterdrücken", MuscleGroup = "Schultern", Description = "Schulterdrücken stehend oder sitzend" },
            new() { Name = "Seitheben", MuscleGroup = "Schultern", Description = "Seitheben mit Kurzhanteln" },
            
            // Arme
            new() { Name = "Bizeps Curls", MuscleGroup = "Arme", Description = "Bizeps Curls mit Kurzhanteln" },
            new() { Name = "Trizeps Dips", MuscleGroup = "Arme", Description = "Trizeps Dips am Barren" }
        };

        foreach (var exercise in defaultExercises)
        {
            exercise.IsSynced = true; // Default exercises don't need sync
            await db.InsertAsync(exercise);
        }
    }
}