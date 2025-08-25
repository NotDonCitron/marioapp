#!/bin/bash

echo "🧪 Testing Minimal MAUI App"
echo "=========================="

# Set Android environment
export ANDROID_HOME=/Users/phhtttps/Library/Android/sdk
export PATH=$PATH:$ANDROID_HOME/platform-tools

# Create a minimal test app
echo "Creating minimal test app..."
dotnet new maui -n TestApp -o test-minimal

cd test-minimal

echo "Building minimal app..."
dotnet build -f net9.0-android

if [ $? -eq 0 ]; then
    echo "✅ Minimal app builds successfully"
    
    echo "Installing on emulator..."
    adb -s emulator-5554 install bin/Debug/net9.0-android/*-Signed.apk
    
    if [ $? -eq 0 ]; then
        echo "✅ Minimal app installed successfully"
        echo "Try launching it from the emulator to test basic MAUI functionality"
    else
        echo "❌ Failed to install minimal app"
    fi
else
    echo "❌ Minimal app build failed"
fi

cd ..