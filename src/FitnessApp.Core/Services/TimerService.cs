using System.Timers;

namespace FitnessApp.Core.Services;

public enum TimerMode
{
    Fixed,
    Random
}

public class TimerSettings
{
    public TimerMode TimerMode { get; set; } = TimerMode.Fixed;
    public int FixedDurationSeconds { get; set; } = 90;
    public int RandomMinSeconds { get; set; } = 60;
    public int RandomMaxSeconds { get; set; } = 180;
}

public class TimerService
{
    private System.Timers.Timer? _timer;
    private int _remainingSeconds;
    private readonly Random _random = new();

    public event EventHandler<int>? TimerTick; // Remaining seconds
    public event EventHandler? TimerCompleted;
    public event EventHandler<int>? TimerStarted; // Total duration

    public bool IsRunning => _timer?.Enabled ?? false;
    public int RemainingSeconds => _remainingSeconds;

    public void StartCountdown(TimerSettings settings)
    {
        int durationInSeconds = settings.TimerMode switch
        {
            TimerMode.Fixed => settings.FixedDurationSeconds,
            TimerMode.Random => _random.Next(settings.RandomMinSeconds, settings.RandomMaxSeconds + 1),
            _ => settings.FixedDurationSeconds
        };

        StartCountdown(durationInSeconds);
    }

    public void StartCountdown(int durationInSeconds)
    {
        Stop();

        _remainingSeconds = durationInSeconds;
        
        _timer = new System.Timers.Timer(1000); // 1 second interval
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Start();

        TimerStarted?.Invoke(this, durationInSeconds);
        TimerTick?.Invoke(this, _remainingSeconds);
    }

    public void Stop()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
    }

    public void Pause()
    {
        _timer?.Stop();
    }

    public void Resume()
    {
        if (_timer != null && _remainingSeconds > 0)
        {
            _timer.Start();
        }
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _remainingSeconds--;
        
        TimerTick?.Invoke(this, _remainingSeconds);

        if (_remainingSeconds <= 0)
        {
            Stop();
            TimerCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Dispose()
    {
        Stop();
    }
}

public static class TimerExtensions
{
    public static string ToMinutesSeconds(this int totalSeconds)
    {
        var minutes = totalSeconds / 60;
        var seconds = totalSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }
}

// Helper class for timer settings management
public class TimerSettingsService
{
    private TimerSettings _settings = new();

    public TimerSettings GetSettings() => _settings;

    public void UpdateSettings(TimerMode mode, int fixedDuration = 90, int randomMin = 60, int randomMax = 180)
    {
        _settings.TimerMode = mode;
        _settings.FixedDurationSeconds = fixedDuration;
        _settings.RandomMinSeconds = randomMin;
        _settings.RandomMaxSeconds = randomMax;
    }
}