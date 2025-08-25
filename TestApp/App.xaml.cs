namespace TestApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new ContentPage 
        { 
            Content = new Label 
            { 
                Text = "Hello, MAUI!",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }
        };
    }
}