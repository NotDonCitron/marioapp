using System.Collections.ObjectModel;

namespace FitnessApp;

public partial class MainPage : ContentPage
{
    private int _steps = 0;
    private int _calories = 0;
    private double _distance = 0.0;
    private ObservableCollection<string> _recentActivities;

    public MainPage()
    {
        InitializeComponent();
        _recentActivities = new ObservableCollection<string>();
        LoadDailyStats();
        LoadRecentActivities();
    }

    private void LoadDailyStats()
    {
        // Simulate some daily stats
        _steps = Random.Shared.Next(1000, 8000);
        _calories = Random.Shared.Next(200, 600);
        _distance = Math.Round(Random.Shared.NextDouble() * 5, 1);
        
        UpdateStatsDisplay();
    }

    private void UpdateStatsDisplay()
    {
        StepsLabel.Text = _steps.ToString();
        CaloriesLabel.Text = _calories.ToString();
        DistanceLabel.Text = _distance.ToString("F1");
    }

    private void LoadRecentActivities()
    {
        // Recent activities will be shown in future version
    }

    private async void OnStartWorkoutClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Start Workout", "Workout feature coming soon!", "OK");
    }

    private async void OnViewStatsClicked(object sender, EventArgs e)
    {
        var message = $"📊 Your Stats:\n\n" +
                     $"Steps: {_steps:N0}\n" +
                     $"Calories: {_calories}\n" +
                     $"Distance: {_distance:F1} km\n\n" +
                     $"Keep up the great work! 💪";
        
        await DisplayAlert("Your Statistics", message, "OK");
    }

    private async void OnSetGoalsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Set Goals", "Goal setting feature coming soon!", "OK");
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Settings", "Settings feature coming soon!", "OK");
    }

    private async void OnAddActivityClicked(object sender, EventArgs e)
    {
        var result = await DisplayActionSheet("Add Activity", "Cancel", null, 
            "🏃 Running", "🏋️ Strength Training", "🚴 Cycling", "🧘 Yoga", "🏊 Swimming");
        
        if (result != null && result != "Cancel")
        {
            // Simulate adding activity
            _steps += Random.Shared.Next(500, 2000);
            _calories += Random.Shared.Next(50, 200);
            _distance += Math.Round(Random.Shared.NextDouble() * 2, 1);
            
            UpdateStatsDisplay();
            
            await DisplayAlert("Activity Added", $"Great job! {result} has been added to your activities.", "OK");
        }
    }
}