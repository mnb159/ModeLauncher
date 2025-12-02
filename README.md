<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>ModeLauncher</title>
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <style>
    body {
      font-family: system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
      line-height: 1.6;
      margin: 0;
      padding: 2rem;
      background-color: #0f172a;
      color: #e5e7eb;
    }
    a { color: #60a5fa; }
    h1, h2, h3 {
      color: #f9fafb;
      margin-top: 1.5rem;
    }
    code {
      background: #020617;
      padding: 0.15rem 0.35rem;
      border-radius: 4px;
      font-size: 0.9em;
    }
    pre {
      background: #020617;
      padding: 1rem;
      border-radius: 8px;
      overflow-x: auto;
      font-size: 0.9em;
    }
    pre code {
      background: transparent;
      padding: 0;
    }
    .tagline {
      font-size: 0.98rem;
      color: #9ca3af;
      margin-bottom: 1.5rem;
    }
    ul {
      padding-left: 1.2rem;
    }
    li {
      margin: 0.2rem 0;
    }
    table {
      border-collapse: collapse;
      width: 100%;
      max-width: 600px;
      margin-top: 0.5rem;
      margin-bottom: 1rem;
    }
    th, td {
      border: 1px solid #1f2933;
      padding: 0.4rem 0.6rem;
      text-align: left;
      font-size: 0.95rem;
    }
    th {
      background-color: #111827;
    }
    .mono {
      font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, "Liberation Mono", "Courier New", monospace;
    }
  </style>
</head>
<body>

  <h1>ModeLauncher</h1>
  <p class="tagline">
    A full-screen WPF launcher for gaming PCs, HTPCs, and couch setups. Choose a mode
    (Gaming, Streaming, Windows, etc.) on boot and auto-launch it after a countdown.
  </p>

  <h2>Features</h2>
  <ul>
    <li>Full-screen, minimal launcher UI</li>
    <li>Configurable modes (Gaming, Streaming, Windows, Chrome, etc.)</li>
    <li>Auto-launch after a configurable countdown</li>
    <li>Keyboard navigation:
      <ul>
        <li><span class="mono">←</span> / <span class="mono">→</span> to switch modes</li>
        <li><span class="mono">Enter</span> to launch</li>
        <li><span class="mono">Ctrl + S</span> opens Settings</li>
        <li><span class="mono">ESC</span> exits launcher</li>
      </ul>
    </li>
    <li>Mouse support – click tiles to select mode</li>
    <li>Built-in settings window:
      <ul>
        <li>Add / remove launch modes</li>
        <li>Edit labels, subtitles, executable paths, and arguments</li>
        <li>Set default mode</li>
        <li>Adjust countdown duration</li>
      </ul>
    </li>
    <li>Icon loading via Windows shell (no System.Drawing dependency)</li>
    <li>Portable, self-contained publish supported</li>
  </ul>

  <h2>Folder Structure</h2>
  <pre><code>ModeLauncher
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
 └─ ModeLauncher.csproj</code></pre>

  <h2>Configuration</h2>

  <p>Configuration is stored in:</p>
  <pre><code>%LOCALAPPDATA%\ModeLauncher\config.json</code></pre>

  <p>Example configuration:</p>
  <pre><code>{
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
}</code></pre>

  <p>The settings window edits this file automatically.</p>

  <h2>Keyboard &amp; Mouse Controls</h2>

  <table>
    <thead>
      <tr>
        <th>Key / Input</th>
        <th>Action</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><span class="mono">Left / Right</span></td>
        <td>Select previous / next mode</td>
      </tr>
      <tr>
        <td><span class="mono">Enter</span></td>
        <td>Launch selected mode</td>
      </tr>
      <tr>
        <td><span class="mono">Ctrl + S</span></td>
        <td>Open Settings window</td>
      </tr>
      <tr>
        <td><span class="mono">ESC</span></td>
        <td>Exit launcher</td>
      </tr>
      <tr>
        <td><span class="mono">Mouse Click</span></td>
        <td>Select mode tile</td>
      </tr>
    </tbody>
  </table>

  <h2>Build</h2>

  <h3>Requirements</h3>
  <ul>
    <li>Visual Studio 2022 or later</li>
    <li>.NET 10 SDK (with WPF workload)</li>
  </ul>

  <h3>Build (Debug / Release)</h3>
  <pre><code>dotnet build</code></pre>

  <h3>Publish (self-contained EXE)</h3>

  <p>Run from the <strong>project</strong> directory (where <code>ModeLauncher.csproj</code> lives):</p>

  <pre><code>dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish</code></pre>

  <p>The output will be in:</p>
  <pre><code>ModeLauncher\publish\</code></pre>

  <p>and will contain <code>ModeLauncher.exe</code> plus required runtime files.</p>

  <h2>Run at Startup</h2>

  <h3>Option 1 – Startup Folder</h3>
  <ol>
    <li>Press <span class="mono">Win + R</span></li>
    <li>Type <span class="mono">shell:startup</span> and press Enter</li>
    <li>Copy a shortcut to <code>ModeLauncher.exe</code> into that folder</li>
  </ol>

  <h3>Option 2 – Task Scheduler</h3>
  <ol>
    <li>Open <strong>Task Scheduler</strong></li>
    <li>Create a basic task</li>
    <li>Trigger: At log on</li>
    <li>Action: Start a program → <code>ModeLauncher.exe</code></li>
    <li>(Optional) Enable “Run with highest privileges”</li>
  </ol>

  <h2>Supported Apps (Examples)</h2>

  <ul>
    <li>Steam (<span class="mono">steam.exe</span>)</li>
    <li>Google Chrome (<span class="mono">chrome.exe</span>)</li>
    <li>GOG Galaxy</li>
    <li>Epic Games Launcher</li>
    <li>Jellyfin Media Player</li>
    <li>VLC / MPC-HC / other players</li>
    <li>RetroArch / emulators</li>
    <li><span class="mono">explorer.exe</span> (Windows Desktop)</li>
  </ul>

  <p>Any executable can be used as a mode.</p>

  <h2>Troubleshooting</h2>

  <h3>Chrome icon not showing</h3>
  <p>Chrome is often installed per-user. Use one of these paths:</p>

  <pre><code>%LOCALAPPDATA%\Google\Chrome\Application\chrome.exe</code></pre>

  <h3>Publish folder is empty</h3>
  <ul>
    <li>Make sure you run <code>dotnet publish</code> from the project directory, not the solution directory.</li>
    <li>Check that <code>ModeLauncher.csproj</code> uses <code>Sdk="Microsoft.NET.Sdk.WindowsDesktop"</code> and <code>&lt;UseWPF&gt;true&lt;/UseWPF&gt;</code>.</li>
  </ul>

  <h3>Settings window layout looks off</h3>
  <p>WPF control defaults vary slightly by theme/OS. You can adjust margins and padding in the XAML for <code>SettingsWindow.xaml</code> if needed.</p>

  <h2>Future Ideas</h2>

  <ul>
    <li>Gamepad navigation support</li>
    <li>Animated transitions and hover effects</li>
    <li>Custom themes and backgrounds per mode</li>
    <li>Automatic detection of installed launchers</li>
    <li>Per-mode startup scripts (e.g., change resolution, audio device)</li>
  </ul>

</body>
</html>
