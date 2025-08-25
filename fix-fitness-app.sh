#!/bin/bash

echo "🔧 Fixing Fitness App Structure"
echo "==============================="

# Set Android environment
export ANDROID_HOME=/Users/phhtttps/Library/Android/sdk
export PATH=$PATH:$ANDROID_HOME/platform-tools

# Copy working MainActivity structure from test app
echo "Copying working MainActivity structure..."
cp test-minimal/Platforms/Android/MainActivity.cs src/FitnessApp/Platforms/Android/MainActivity.cs

# Update namespace in MainActivity
sed -i '' 's/namespace TestApp;/namespace FitnessApp;/g' src/FitnessApp/Platforms/Android/MainActivity.cs

# Copy working MainApplication structure
cp test-minimal/Platforms/Android/MainApplication.cs src/FitnessApp/Platforms/Android/MainApplication.cs

# Update namespace in MainApplication
sed -i '' 's/namespace TestApp;/namespace FitnessApp;/g' src/FitnessApp/Platforms/Android/MainApplication.cs

echo "Building fixed app..."
dotnet clean src/FitnessApp/FitnessApp.csproj
dotnet build src/FitnessApp/FitnessApp.csproj -f net9.0-android

if [ $? -eq 0 ]; then
    echo "✅ Fixed app builds successfully"
    
    echo "Installing fixed app..."
    adb -s emulator-5554 uninstall com.fitnessapp.tracker
    adb -s emulator-5554 install src/FitnessApp/bin/Debug/net9.0-android/*-Signed.apk
    
    if [ $? -eq 0 ]; then
        echo "✅ Fixed app installed successfully"
        echo "Testing app launch..."
        
        # Clear logs and launch
        adb -s emulator-5554 logcat -c
        adb -s emulator-5554 shell am start -n com.fitnessapp.tracker/crc64fcf28c0e24b4cc31.MainActivity
        
        sleep 3
        
        # Check for crashes
        CRASH_COUNT=$(adb -s emulator-5554 logcat -d | grep -c "AndroidRuntime.*FATAL")
        
        if [ $CRASH_COUNT -eq 0 ]; then
            echo "🎉 App launched successfully without crashes!"
        else
            echo "❌ App still crashing. Checking logs..."
            adb -s emulator-5554 logcat -d | grep -E "(FitnessApp|AndroidRuntime|FATAL)" | tail -10
        fi
    else
        echo "❌ Failed to install fixed app"
    fi
else
    echo "❌ Fixed app build failed"
fi