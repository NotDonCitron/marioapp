# Fitness Tracker App

Eine plattformübergreifende Fitness-App für Android und Windows, entwickelt mit .NET MAUI und inspiriert von den besten Open Source Fitness-Projekten.

## 🎯 Projektübersicht

Dieses Projekt basiert auf einer detaillierten Analyse von Open Source Fitness-Apps und implementiert bewährte Patterns für:
- Workout-Planung und -Tracking
- Offline-First-Architektur
- Echtzeit-Timer-Funktionalität
- Fortschritts-Visualisierung

## 🏗️ Architektur

### Frontend
- **.NET MAUI** - Cross-platform UI Framework
- **MVVM Pattern** mit CommunityToolkit.Mvvm
- **SQLite** für lokale Datenspeicherung
- **Dependency Injection** für lose Kopplung

### Backend (geplant)
- **Supabase** - PostgreSQL-basierte Backend-as-a-Service
- **Row Level Security** für Datenschutz
- **Realtime Synchronisation** zwischen Geräten

## 📱 Features (MVP)

### ✅ Implementiert
- **Dashboard** mit Trainingsstatistiken
- **Live-Workout-Tracker** mit Echtzeit-Timer
- **Workout-Planer** - Vollständige CRUD-Funktionalität
  - Trainingspläne erstellen/bearbeiten/löschen
  - Übungen zu Plänen hinzufügen/entfernen
  - Übungsreihenfolge ändern (Drag & Drop)
  - Pläne direkt starten
  - Suchfunktion für Übungen
- **Trainingsverlauf & Statistiken** - Vollständige Analytics
  - Detaillierte Workout-Historie mit Zeitfiltern
  - Persönliche Rekorde (PRs) mit 1RM-Berechnung
  - Umfassende Statistiken (Volumen, Streak, Ø Dauer)
  - Workout-Details mit allen Sätzen
  - Zeitraum-Filter (Woche, Monat, 3 Monate)
- **Übungsauswahl** aus vorgefertigter Datenbank (13 Übungen)
- **Satz-Protokollierung** mit Gewicht und Wiederholungen
- **Rest-Timer** mit konfigurierbaren Intervallen (fest/zufällig)
- **Datenmodelle** für Users, Exercises, Workouts, Sets, Plans
- **SQLite-Integration** mit verschlüsselter Speicherung
- **MVVM-Architektur** mit Commands und Data Binding
- **Value Converters** für UI-Bindings
- **Responsive UI** mit Dialogs und Overlays

### 🚧 In Entwicklung
- Timer-Einstellungen-Page
- Supabase-Integration für Cloud-Sync
- Erweiterte Übungsdatenbank
- Charts und Visualisierungen

### 🔮 Geplant (v2)
- Offline-Synchronisation
- Fortschritts-Charts
- Gamification (Badges, Streaks)
- Data Export/Import

## 🚀 Quick Start

### Voraussetzungen
- Visual Studio 2022 mit .NET MAUI Workload
- .NET 8.0 SDK
- Android SDK (für Android-Entwicklung)
- Windows 10/11 SDK (für Windows-Entwicklung)

### Installation
```bash
# Repository klonen
git clone <repository-url>
cd fitness-app

# Dependencies installieren
dotnet restore

# App starten (Android)
dotnet build -f net8.0-android
dotnet run -f net8.0-android

# App starten (Windows)
dotnet build -f net8.0-windows10.0.19041.0
dotnet run -f net8.0-windows10.0.19041.0
```

## 📊 Datenmodell

```
Users (Benutzer)
├── WorkoutPlans (Trainingspläne)
│   └── PlanExercises (Plan-Übungen)
│       └── Exercises (Übungen)
└── WorkoutSessions (Trainingseinheiten)
    └── SetLogs (Satz-Protokolle)
        └── Exercises (Übungen)
```

## 🎨 UI/UX Design

- **Material Design** inspirierte Komponenten
- **Responsive Layout** für verschiedene Bildschirmgrößen
- **Dark/Light Theme** Support
- **Accessibility** Features

## 🔧 Technische Details

### Verwendete NuGet Packages
- `Microsoft.Maui.Controls` - MAUI Framework
- `CommunityToolkit.Maui` - UI Controls und Behaviors
- `CommunityToolkit.Mvvm` - MVVM Toolkit
- `SQLite-net-pcl` - SQLite ORM
- `supabase-csharp` - Supabase Client (geplant)

### Projektstruktur
```
src/
├── FitnessApp/              # MAUI App
│   ├── Views/               # XAML Pages
│   ├── ViewModels/          # MVVM ViewModels
│   └── Resources/           # Styles, Images, etc.
└── FitnessApp.Core/         # Shared Logic
    ├── Models/              # Data Models
    └── Services/            # Business Logic
```

## 🤝 Inspiration

Dieses Projekt wurde inspiriert von folgenden Open Source Projekten:
- **FitTrackee** - Datenbank-Schema und API-Design
- **FitoTrack** - Offline-First-Patterns und Android UI
- **OpenTracks** - Performance-Optimierung und Datenmodelle

## 📈 Roadmap

### Phase 1: Core Features (4-6 Wochen) ✅ ABGESCHLOSSEN
- [x] Grundlegende App-Struktur
- [x] Datenmodelle und SQLite-Integration
- [x] Dashboard mit Statistiken
- [x] Workout-Planer (vollständig)
- [x] Live-Training-Tracker (vollständig)
- [x] Trainingsverlauf & Analytics (vollständig)

### Phase 2: Cloud Integration (2-3 Wochen)
- [ ] Supabase-Setup
- [ ] User Authentication
- [ ] Cloud-Synchronisation
- [ ] Offline-Conflict-Resolution

### Phase 3: Polish & Features (2-4 Wochen)
- [ ] Erweiterte Statistiken
- [ ] Gamification
- [ ] Performance-Optimierung
- [ ] App Store Deployment

## 🐛 Known Issues

- Icons für TabBar fehlen noch
- Supabase-Integration steht noch aus
- Search-Funktionalität in Übungsauswahl noch nicht implementiert
- Workout-Planer noch nicht vollständig implementiert

## 📝 Lizenz

Dieses Projekt ist für private Nutzung entwickelt und nicht für kommerzielle Zwecke bestimmt.

## 🙋‍♂️ Support

Bei Fragen oder Problemen, erstelle ein Issue oder kontaktiere das Entwicklungsteam.