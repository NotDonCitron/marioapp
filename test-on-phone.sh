#!/bin/bash

echo "📱 Testing MAUI Fitness App on Real Device"
echo "=========================================="

# Set Android environment
export ANDROID_HOME=/Users/phhtttps/Library/Android/sdk
export PATH=$PATH:$ANDROID_HOME/platform-tools

# Get phone device ID
PHONE_DEVICE=$(adb devices | grep "39081FDJG003UG" | head -1 | awk '{print $1}')

if [ -z "$PHONE_DEVICE" ]; then
    echo "❌ Phone not found. Make sure wireless debugging is enabled."
    exit 1
fi

echo "📱 Found device: $PHONE_DEVICE"

echo "🔨 Building latest version..."
dotnet build src/FitnessApp/FitnessApp.csproj -f net9.0-android

if [ $? -eq 0 ]; then
    echo "✅ Build successful"
    
    echo "📲 Installing on phone..."
    adb -s "$PHONE_DEVICE" install -r src/FitnessApp/bin/Debug/net9.0-android/*-Signed.apk
    
    if [ $? -eq 0 ]; then
        echo "✅ App installed successfully on phone"
        
        echo "🚀 Launching app..."
        adb -s "$PHONE_DEVICE" logcat -c
        adb -s "$PHONE_DEVICE" shell am start -n com.fitnessapp.tracker/crc64fcf28c0e24b4cc31.MainActivity
        
        echo "⏳ Waiting for app to start..."
        sleep 3
        
        # Check for crashes
        CRASH_COUNT=$(adb -s "$PHONE_DEVICE" logcat -d | grep -c "AndroidRuntime.*FATAL")
        
        if [ $CRASH_COUNT -eq 0 ]; then
            echo "🎉 App launched successfully on your phone!"
            echo ""
            echo "📋 Next steps:"
            echo "1. Check your phone - the Fitness Tracker app should be open"
            echo "2. Navigate through the tabs: Dashboard, Planer, Training, Verlauf"
            echo "3. Test creating workout plans and tracking exercises"
            echo ""
            echo "🔍 To monitor logs while testing:"
            echo "adb -s $PHONE_DEVICE logcat | grep FitnessApp"
        else
            echo "❌ App crashed on launch. Checking logs..."
            adb -s "$PHONE_DEVICE" logcat -d | grep -E "(FitnessApp|AndroidRuntime|FATAL)" | tail -10
        fi
    else
        echo "❌ Failed to install app on phone"
    fi
else
    echo "❌ Build failed"
fi

echo ""
echo "📱 Device testing complete!"