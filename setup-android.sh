#!/bin/bash

echo "🤖 Android SDK Setup für MAUI"
echo "=============================="

# Check if Android Studio is installed
if ! command -v /Applications/Android\ Studio.app/Contents/MacOS/studio &> /dev/null; then
    echo "❌ Android Studio not found. Please install it first:"
    echo "   brew install --cask android-studio"
    exit 1
fi

echo "✅ Android Studio found"

# Set Android SDK path
export ANDROID_SDK_ROOT="$HOME/Library/Android/sdk"
export ANDROID_HOME="$ANDROID_SDK_ROOT"
export PATH="$PATH:$ANDROID_SDK_ROOT/tools:$ANDROID_SDK_ROOT/platform-tools"

echo "📱 Android SDK Path: $ANDROID_SDK_ROOT"

# Check if SDK exists
if [ ! -d "$ANDROID_SDK_ROOT" ]; then
    echo "⚠️  Android SDK not found at $ANDROID_SDK_ROOT"
    echo ""
    echo "🔧 Manual Setup Required:"
    echo "1. Open Android Studio"
    echo "2. Go to Preferences > Appearance & Behavior > System Settings > Android SDK"
    echo "3. Install Android SDK (API 34 or higher)"
    echo "4. Install Android SDK Build-Tools"
    echo "5. Install Android Emulator"
    echo ""
    echo "Then run this script again."
    
    # Open Android Studio
    echo "Opening Android Studio..."
    open -a "Android Studio"
    exit 1
fi

echo "✅ Android SDK found"

# Check for required components
if [ ! -d "$ANDROID_SDK_ROOT/platform-tools" ]; then
    echo "❌ Android Platform Tools missing"
    echo "   Install via Android Studio SDK Manager"
    exit 1
fi

if [ ! -d "$ANDROID_SDK_ROOT/build-tools" ]; then
    echo "❌ Android Build Tools missing"
    echo "   Install via Android Studio SDK Manager"
    exit 1
fi

echo "✅ Android Platform Tools found"
echo "✅ Android Build Tools found"

# List available emulators
echo ""
echo "📱 Available Android Emulators:"
if command -v "$ANDROID_SDK_ROOT/emulator/emulator" &> /dev/null; then
    "$ANDROID_SDK_ROOT/emulator/emulator" -list-avds
else
    echo "   No emulators found. Create one in Android Studio."
fi

echo ""
echo "🎯 Next Steps:"
echo "1. Create an Android Virtual Device (AVD) in Android Studio"
echo "2. Or connect a physical Android device with USB debugging"
echo "3. Run: ./test-android-app.sh"

# Export environment variables for current session
echo ""
echo "🔧 Environment Setup:"
echo "export ANDROID_SDK_ROOT=\"$ANDROID_SDK_ROOT\""
echo "export ANDROID_HOME=\"$ANDROID_SDK_ROOT\""
echo "export PATH=\"\$PATH:\$ANDROID_SDK_ROOT/tools:\$ANDROID_SDK_ROOT/platform-tools\""
echo ""
echo "Add these to your ~/.zshrc for permanent setup"