#!/bin/bash

echo "📦 Fitness App - Release Preparation"
echo "===================================="

# Create release directory
mkdir -p releases

# Version info
VERSION="1.0.0"
BUILD_NUMBER=$(date +%Y%m%d%H%M)

echo "🏷️  Version: $VERSION"
echo "🔢 Build: $BUILD_NUMBER"

# Update version in project file
sed -i '' "s/<ApplicationDisplayVersion>.*<\/ApplicationDisplayVersion>/<ApplicationDisplayVersion>$VERSION<\/ApplicationDisplayVersion>/" src/FitnessApp/FitnessApp.csproj
sed -i '' "s/<ApplicationVersion>.*<\/ApplicationVersion>/<ApplicationVersion>$BUILD_NUMBER<\/ApplicationVersion>/" src/FitnessApp/FitnessApp.csproj

echo "✅ Version updated in project file"

# Clean previous builds
echo "🧹 Cleaning previous builds..."
dotnet clean src/FitnessApp/FitnessApp.csproj -c Release

# Build for Android Release
echo "🤖 Building Android Release..."
if dotnet publish src/FitnessApp/FitnessApp.csproj -f net9.0-android -c Release -o releases/android; then
    echo "✅ Android release build successful"
    
    # Find the APK
    APK_FILE=$(find releases/android -name "*.apk" | head -1)
    if [ -n "$APK_FILE" ]; then
        echo "📱 APK created: $APK_FILE"
        
        # Get APK info
        APK_SIZE=$(du -h "$APK_FILE" | cut -f1)
        echo "📏 APK size: $APK_SIZE"
    fi
else
    echo "❌ Android release build failed"
fi

# Build for iOS Release (if Xcode available)
if command -v xcodebuild &> /dev/null; then
    echo "🍎 Building iOS Release..."
    if dotnet publish src/FitnessApp/FitnessApp.csproj -f net9.0-ios -c Release -o releases/ios; then
        echo "✅ iOS release build successful"
    else
        echo "❌ iOS release build failed"
    fi
else
    echo "⚠️  Xcode not found, skipping iOS release build"
fi

# Create release notes
cat > releases/RELEASE_NOTES.md << EOF
# Fitness Tracker v$VERSION

## 🏋️‍♂️ Features

### Core Functionality
- ✅ Complete workout tracking system
- ✅ 13 pre-configured exercises across 5 muscle groups
- ✅ Real-time set logging (weight + reps)
- ✅ Automatic rest timer (90 seconds)
- ✅ Workout session management

### Statistics & Progress
- ✅ Personal records (1RM calculation using Epley formula)
- ✅ Total volume tracking
- ✅ Workout streak counter
- ✅ Comprehensive workout history
- ✅ Exercise-specific progress tracking

### User Experience
- ✅ Modern, intuitive interface
- ✅ Custom fitness-themed icons
- ✅ Responsive design for all screen sizes
- ✅ Offline-first architecture
- ✅ Fast SQLite database

### Technical
- ✅ .NET MAUI cross-platform framework
- ✅ SQLite local database
- ✅ MVVM architecture
- ✅ Dependency injection
- ✅ Optimized for performance

## 🎯 Supported Platforms
- Android 5.0+ (API 21+)
- iOS 11.0+
- macOS 10.15+ (via Mac Catalyst)

## 📱 Installation
- Android: Install APK directly or via Google Play Store
- iOS: Install via App Store

## 🚀 Getting Started
1. Open the app
2. Tap "Training starten" on the dashboard
3. Select an exercise
4. Log your sets (weight + reps)
5. Complete your workout
6. View progress in the history tab

## 🏆 What's Next
- Cloud sync with Supabase
- Advanced charts and visualizations
- Custom exercise creation
- Social features and challenges
- Premium statistics

---
Built with ❤️ and 💪 for fitness enthusiasts
EOF

echo ""
echo "📋 Release Summary:"
echo "=================="
echo "Version: $VERSION"
echo "Build: $BUILD_NUMBER"
echo "Release notes: releases/RELEASE_NOTES.md"
echo ""
echo "🚀 Ready for deployment!"
echo ""
echo "Next steps:"
echo "1. Test the release build on device"
echo "2. Upload to Google Play Console"
echo "3. Submit for review"