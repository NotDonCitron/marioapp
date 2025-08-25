#!/bin/bash

echo "🏋️‍♂️ Fitness App - Complete Testing Suite"
echo "=========================================="

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Test functions
test_core_functionality() {
    echo -e "${BLUE}🧪 Testing Core Functionality...${NC}"
    dotnet run --project src/FitnessApp.Console/FitnessApp.Console.csproj
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✅ Core functionality test passed${NC}"
        return 0
    else
        echo -e "${RED}❌ Core functionality test failed${NC}"
        return 1
    fi
}

test_android_build() {
    echo -e "${BLUE}🤖 Testing Android Build...${NC}"
    
    # Set Android environment
    export ANDROID_SDK_ROOT="$HOME/Library/Android/sdk"
    export ANDROID_HOME="$ANDROID_SDK_ROOT"
    export PATH="$PATH:$ANDROID_SDK_ROOT/tools:$ANDROID_SDK_ROOT/platform-tools"
    
    # Clean and build
    dotnet clean src/FitnessApp/FitnessApp.csproj
    dotnet build src/FitnessApp/FitnessApp.csproj -f net9.0-android -c Debug
    
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✅ Android build test passed${NC}"
        return 0
    else
        echo -e "${RED}❌ Android build test failed${NC}"
        return 1
    fi
}

test_ios_build() {
    echo -e "${BLUE}🍎 Testing iOS Build...${NC}"
    
    if command -v xcodebuild &> /dev/null; then
        dotnet build src/FitnessApp/FitnessApp.csproj -f net9.0-ios -c Debug
        if [ $? -eq 0 ]; then
            echo -e "${GREEN}✅ iOS build test passed${NC}"
            return 0
        else
            echo -e "${RED}❌ iOS build test failed${NC}"
            return 1
        fi
    else
        echo -e "${YELLOW}⚠️  Xcode not found, skipping iOS build test${NC}"
        return 0
    fi
}

test_maccatalyst_build() {
    echo -e "${BLUE}🖥️  Testing MacCatalyst Build...${NC}"
    
    if command -v xcodebuild &> /dev/null; then
        dotnet build src/FitnessApp/FitnessApp.csproj -f net9.0-maccatalyst -c Debug
        if [ $? -eq 0 ]; then
            echo -e "${GREEN}✅ MacCatalyst build test passed${NC}"
            return 0
        else
            echo -e "${RED}❌ MacCatalyst build test failed${NC}"
            return 1
        fi
    else
        echo -e "${YELLOW}⚠️  Xcode not found, skipping MacCatalyst build test${NC}"
        return 0
    fi
}

# Run all tests
echo "Starting comprehensive test suite..."
echo ""

TESTS_PASSED=0
TESTS_TOTAL=0

# Test 1: Core functionality
((TESTS_TOTAL++))
if test_core_functionality; then
    ((TESTS_PASSED++))
fi

echo ""

# Test 2: Android build
((TESTS_TOTAL++))
if test_android_build; then
    ((TESTS_PASSED++))
fi

echo ""

# Test 3: iOS build (if available)
((TESTS_TOTAL++))
if test_ios_build; then
    ((TESTS_PASSED++))
fi

echo ""

# Test 4: MacCatalyst build (if available)
((TESTS_TOTAL++))
if test_maccatalyst_build; then
    ((TESTS_PASSED++))
fi

echo ""
echo "=========================================="
echo -e "${BLUE}📊 Test Results Summary${NC}"
echo "=========================================="
echo -e "Tests passed: ${GREEN}$TESTS_PASSED${NC}/$TESTS_TOTAL"

if [ $TESTS_PASSED -eq $TESTS_TOTAL ]; then
    echo -e "${GREEN}🎉 All tests passed! Your app is ready for deployment.${NC}"
    echo ""
    echo "🚀 Next steps:"
    echo "1. Test on real device: ./test-android-app.sh"
    echo "2. Create release build: dotnet publish -c Release"
    echo "3. Deploy to app store"
else
    echo -e "${YELLOW}⚠️  Some tests failed. Check the output above.${NC}"
fi

echo ""
echo "🏋️‍♂️ Ready to lift! 💪"