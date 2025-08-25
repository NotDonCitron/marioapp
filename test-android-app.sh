#!/bin/bash

echo "🏋️‍♂️ Fitness App - Android Testing"
echo "=================================="

# Set Android environment
export ANDROID_SDK_ROOT="$HOME/Library/Android/sdk"
export ANDROID_HOME="$ANDROID_SDK_ROOT"
export PATH="$PATH:$ANDROID_SDK_ROOT/tools:$ANDROID_SDK_ROOT/platform-tools"

# Check if Android SDK is available
if [ ! -d "$ANDROID_SDK_ROOT" ]; then
    echo "❌ Android SDK not found. Run ./setup-android.sh first"
    exit 1
fi

echo "✅ Android SDK found at $ANDROID_SDK_ROOT"

# Check for connected devices or emulators
echo "📱 Checking for Android devices..."
if command -v adb &> /dev/null; then
    DEVICES=$(adb devices | grep -v "List of devices" | grep -v "^$" | wc -l)
    if [ $DEVICES -eq 0 ]; then
        echo "⚠️  No Android devices found"
        echo ""
        echo "Options:"
        echo "1. Connect Android device with USB debugging enabled"
        echo "2. Start Android emulator:"
        echo "   - Open Android Studio"
        echo "   - Go to Tools > AVD Manager"
        echo "   - Start an emulator"
        echo ""
        read -p "Press Enter when device is ready..."
    else
        echo "✅ Found $DEVICES Android device(s)"
        adb devices
    fi
else
    echo "❌ ADB not found. Check Android SDK installation"
    exit 1
fi

echo ""
echo "🔨 Building Fitness App for Android..."

# Clean and restore
dotnet clean src/FitnessApp/FitnessApp.csproj
dotnet restore src/FitnessApp/FitnessApp.csproj

# Build for Android
echo "Building Android APK..."
dotnet build src/FitnessApp/FitnessApp.csproj -f net9.0-android -c Debug

if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi

echo "✅ Build successful!"

# Deploy and run
echo ""
echo "🚀 Deploying to Android device..."
dotnet run --project src/FitnessApp/FitnessApp.csproj -f net9.0-android

echo ""
echo "🎉 App should now be running on your Android device!"
echo ""
echo "📋 Testing Checklist:"
echo "□ App opens without crashes"
echo "□ Dashboard shows welcome message"
echo "□ Navigation tabs work (Dashboard, Planer, Training, Verlauf)"
echo "□ Icons are displayed correctly"
echo "□ Can start a workout"
echo "□ Can select exercises"
echo "□ Can log sets (weight + reps)"
echo "□ Timer works"
echo "□ Can complete workout"
echo "□ Statistics are calculated"
echo "□ Workout appears in history"