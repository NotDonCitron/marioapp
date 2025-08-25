#!/bin/bash

echo "🤖 Simple Android Build Test"
echo "============================"

# Set Android environment
export ANDROID_SDK_ROOT="$HOME/Library/Android/sdk"
export ANDROID_HOME="$ANDROID_SDK_ROOT"

echo "Building without AOT optimizations for compatibility..."

# Clean first
dotnet clean src/FitnessApp/FitnessApp.csproj

# Simple build without AOT
dotnet build src/FitnessApp/FitnessApp.csproj -f net9.0-android -c Debug -p:AndroidEnableProfiledAot=false -p:RunAOTCompilation=false

if [ $? -eq 0 ]; then
    echo "✅ Simple Android build successful!"
    echo ""
    echo "🚀 Ready for device testing once Android Studio setup is complete"
    echo ""
    echo "Next steps:"
    echo "1. Complete Android Studio SDK installation"
    echo "2. Create/start Android emulator"
    echo "3. Run: dotnet run --project src/FitnessApp/FitnessApp.csproj -f net9.0-android"
else
    echo "❌ Build failed - waiting for Android SDK setup"
fi