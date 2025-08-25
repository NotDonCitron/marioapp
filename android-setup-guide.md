# 🤖 Android Setup für Fitness App

## Schritt 1: SDK Komponenten installieren

In Android Studio SDK Manager, installiere:

### ✅ Pflicht (Required):
- **Android SDK Platform 36** (Android 15)
- **Android SDK Build-Tools 36** 
- **Android SDK Platform-Tools**
- **Android Emulator**

### ✅ Für Emulator:
- **Google Play ARM 64 v8a System Image** (Android 36)

### ⚠️ Optional:
- Sources for Android 36 (nur für Debugging)
- Build-Tools 36.1-rc1 (nur für neueste Features)

## Schritt 2: AVD (Android Virtual Device) erstellen

1. In Android Studio: **Tools > AVD Manager**
2. **Create Virtual Device**
3. Wähle ein **Phone** (z.B. Pixel 7)
4. System Image: **Android 36 (Google Play)**
5. AVD Name: **FitnessApp_Test**
6. **Finish**

## Schritt 3: Emulator starten

1. AVD Manager öffnen
2. **FitnessApp_Test** starten (▶️ Button)
3. Warten bis Android vollständig geladen ist

## Schritt 4: App testen

```bash
# In Terminal:
./test-android-app.sh
```

## 🎯 Minimale Konfiguration

Wenn Speicherplatz knapp ist, reicht auch:
- Android SDK Platform 34 (Android 14) 
- Build-Tools 34
- Google Play ARM 64 v8a System Image (API 34)

Die App funktioniert ab Android API 21 (Android 5.0).

## 🚀 Nach Installation

1. Emulator starten
2. `./test-android-app.sh` ausführen
3. App sollte automatisch installiert und gestartet werden
4. Erstes Workout protokollieren! 💪