📦 ModeLauncher

A full-screen WPF launcher designed for gaming PCs, home theater setups, and living-room systems.
It provides a simple “Choose Mode” startup screen that automatically launches a selected application (Steam, Jellyfin, Windows Desktop, Chrome, etc.) after a countdown — with keyboard and mouse support and a built-in settings editor.

Perfect for couch PCs and HTPC builds.

🎯 Features

Full-screen, minimal launcher UI

Configurable modes (Gaming, Streaming, Windows, Chrome, etc.)

Auto-launch after countdown

Keyboard navigation

← / → to switch mode

Enter to launch

Ctrl+S opens Settings

ESC exits launcher

Mouse support — click tiles to select

Built-in settings window

Add / remove launch modes

Change executable paths

Edit labels / arguments

Choose default mode

Change countdown duration

Auto-refresh icons using Windows shell extraction

No System.Drawing dependency (works in publish builds)

Portable — self-contained single-file publish supported

📁 Folder Structure
ModeLauncher
 ├─ Converters/
 │   └─ BoolToHighlightConverter.cs
 ├─ Helpers/
 │   └─ BaseNotifier.cs
 ├─ Models/
 │   └─ LaunchMode.cs
 │   └─ LauncherConfig.cs
 ├─ Services/
 │   └─ ConfigService.cs
 │   └─ IconService.cs
 ├─ ViewModels/
 │   └─ ModeItem.cs
 │   └─ SettingsViewModel.cs
 ├─ LauncherWindow.xaml
 ├─ LauncherWindow.xaml.cs
 ├─ SettingsWindow.xaml
 ├─ SettingsWindow.xaml.cs
 └─ ModeLauncher.csproj

⚙️ Configuration File

The launcher stores its config here:

%LOCALAPPDATA%\ModeLauncher\config.json


Example:

{
  "DefaultModeId": "gaming",
  "CountdownSeconds": 5,
  "Modes": [
    {
      "Id": "gaming",
      "Label": "Gaming",
      "Subtitle": "Steam Big Picture",
      "ExecutablePath": "C:\\Program Files (x86)\\Steam\\steam.exe",
      "Arguments": "-bigpicture"
    },
    {
      "Id": "streaming",
      "Label": "Streaming",
      "Subtitle": "Jellyfin Player",
      "ExecutablePath": "C:\\Program Files\\Jellyfin Media Player\\jellyfinmediaplayer.exe"
    },
    {
      "Id": "windows",
      "Label": "Windows",
      "Subtitle": "Desktop Mode",
      "ExecutablePath": "explorer.exe"
    }
  ]
}


The settings window rewrites this file automatically.

⌨️ Keyboard Controls
Key	Action
Left / Right	Select mode
Enter	Launch selected mode
Ctrl + S	Open Settings
ESC	Exit launcher
Mouse Click	Select mode
🚀 How to Build
Requirements

Visual Studio 2022/2026

.NET 10 SDK (with WPF support)

Build
dotnet build

Publish (self-contained EXE)

Run from project folder, not solution folder:

dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish


Result will appear in:

ModeLauncher/publish/

🖥️ Set ModeLauncher to Run at Startup
Option 1 — Startup Folder (recommended)

Create a shortcut:

shell:startup


Place ModeLauncher.exe there.

Option 2 — Task Scheduler (for admin/system setups)

Triggers → At logon
Action → Start Program → ModeLauncher.exe
Run with highest privileges → ON

🧪 Known Supported Apps

Steam (steam.exe)

Chrome (chrome.exe)

GOG Galaxy

Epic Games Launcher

Jellyfin Media Player

MPC-HC / VLC

RetroArch

Explorer.exe (Windows Desktop)

Anything executable works.

🛠️ Troubleshooting
❗ Chrome icon not showing

Chrome is often installed per-user.

Use:

%LOCALAPPDATA%\Google\Chrome\Application\chrome.exe

❗ Publish output folder empty

Ensure publish is run inside project directory, not solution folder.

❗ Settings window buttons misaligned

Your version of WPF may apply different default padding.
You may restyle the controls via Styles.xaml.

💡 Future Enhancements (optional ideas)

Gamepad navigation

Animated transitions for tiles

Theme packs

Steam artwork auto-loading

Custom backgrounds per mode

Auto-detect installed apps
