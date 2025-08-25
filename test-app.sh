#!/bin/bash

echo "🏋️‍♂️ Fitness App Test Script"
echo "=============================="

# Check if .NET MAUI workload is installed
echo "Checking MAUI workload..."
if dotnet workload list | grep -q "maui"; then
    echo "✅ MAUI workload is installed"
else
    echo "❌ MAUI workload not found. Installing..."
    dotnet workload install maui
fi

# Restore packages
echo "Restoring packages..."
dotnet restore src/FitnessApp/FitnessApp.csproj

# Build for Android
echo "Building for Android..."
dotnet build src/FitnessApp/FitnessApp.csproj -f net9.0-android

if [ $? -eq 0 ]; then
    echo "✅ Android build successful!"
    echo ""
    echo "To run on Android device:"
    echo "dotnet run --project src/FitnessApp/FitnessApp.csproj -f net9.0-android"
else
    echo "❌ Android build failed"
fi

# Build for Windows (if on Windows)
if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "win32" ]]; then
    echo "Building for Windows..."
    dotnet build src/FitnessApp/FitnessApp.csproj -f net9.0-windows10.0.19041.0
    
    if [ $? -eq 0 ]; then
        echo "✅ Windows build successful!"
        echo ""
        echo "To run on Windows:"
        echo "dotnet run --project src/FitnessApp/FitnessApp.csproj -f net9.0-windows10.0.19041.0"
    else
        echo "❌ Windows build failed"
    fi
fi

echo ""
echo "🚀 Next steps:"
echo "1. Connect Android device with USB debugging enabled"
echo "2. Run: dotnet run --project src/FitnessApp/FitnessApp.csproj -f net9.0-android"
echo "3. Start logging your first workout!"