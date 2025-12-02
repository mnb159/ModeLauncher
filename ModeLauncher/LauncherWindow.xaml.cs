using ModeLauncher.Models;
using ModeLauncher.Services;
using ModeLauncher.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ModeLauncher
{
    public partial class LauncherWindow : Window
    {
        public ObservableCollection<ModeItem> Modes { get; set; } = new();
        public string CountdownText => _timeLeft > 0
            ? $"Launching default mode in {_timeLeft} seconds..."
            : "Selection active";

        private int _timeLeft;
        private DispatcherTimer? _timer;
        private LauncherConfig? _config;
        private ModeItem? _selectedMode;

        public LauncherWindow()
        {
            InitializeComponent();
            LoadConfig();
            SetupUI();
        }

        // -------------------------
        // CONFIG & MODE LOADING
        // -------------------------
        private void LoadConfig()
        {
            var service = new ConfigService();
            _config = service.Load();

            _timeLeft = _config.CountdownSeconds;

            Modes.Clear();

            foreach (var mode in _config.Modes)
            {
                var item = new ModeItem
                {
                    Id = mode.Id,
                    Label = mode.Label,
                    Subtitle = mode.Subtitle,
                    ExecutablePath = mode.ExecutablePath,
                    Arguments = mode.Arguments
                };

                // extract icon from the executable
                item.Icon = IconService.FromExe(item.ExecutablePath);

                Modes.Add(item);
            }

            _selectedMode = Modes.FirstOrDefault(m => m.Id == _config.DefaultModeId)
                             ?? Modes.FirstOrDefault();

            if (_selectedMode != null)
                _selectedMode.IsSelected = true;
        }


        // -------------------------
        // UI SETUP & COUNTDOWN
        // -------------------------
        private void SetupUI()
        {
            DataContext = this;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += (s, e) =>
            {
                _timeLeft--;

                if (_timeLeft <= 0)
                {
                    _timer.Stop();
                    LaunchSelected();
                    return;
                }

                OnPropertyChanged(nameof(CountdownText));
            };

            _timer.Start();
        }

        private void CancelCountdown()
        {
            _timer?.Stop();
            _timeLeft = 0;
            OnPropertyChanged(nameof(CountdownText));
        }

        // -------------------------
        // GRID LOADED = SET FOCUS
        // -------------------------
        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            Root.Focus();
        }

        // -------------------------
        // KEYBOARD NAVIGATION
        // -------------------------
        private void Root_KeyDown(object sender, KeyEventArgs e)
        {
            CancelCountdown();

            if (e.Key == Key.Escape)
            {
                Close();
                return;
            }

            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                OpenSettings();
                return;
            }

            if (e.Key == Key.Left)
                Move(-1);

            if (e.Key == Key.Right)
                Move(1);

            if (e.Key == Key.Enter)
            {
                LaunchSelected();
            }
        }

        // -------------------------
        // MOUSE CANCELS COUNTDOWN
        // -------------------------
        private void Root_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CancelCountdown();
        }

        // -------------------------
        // MODE SELECTION MOVEMENT
        // -------------------------
        private void Move(int delta)
        {
            if (Modes.Count == 0) return;

            int currentIndex = _selectedMode != null ? Modes.IndexOf(_selectedMode) : 0;
            int newIndex = (currentIndex + delta + Modes.Count) % Modes.Count;

            if (_selectedMode != null)
                _selectedMode.IsSelected = false;

            _selectedMode = Modes[newIndex];
            _selectedMode.IsSelected = true;
        }

        // -------------------------
        // MODE LAUNCHING
        // -------------------------
        private void LaunchSelected()
        {
            if (_selectedMode == null) return;

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _selectedMode.ExecutablePath,
                    Arguments = _selectedMode.Arguments ?? "",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to launch {_selectedMode.Label}:\n{ex.Message}");
            }

            Close();
        }

        // -------------------------
        // SETTINGS WINDOW
        // -------------------------
        private void OpenSettings()
        {
            // Remove topmost so the settings window can appear in front
            this.Topmost = false;

            var service = new ConfigService();
            var cfg = service.Load();

            var vm = new SettingsViewModel(cfg, service);

            var win = new SettingsWindow
            {
                DataContext = vm,
                Owner = this
            };

            win.ShowDialog();

            // Restore launcher topmost after settings closes
            this.Topmost = true;

            // Reload updated config
            LoadConfig();
            SetupUI();
        }

        // -------------------------
        // SIMPLE PROPERTY REFRESH
        // -------------------------
        protected void OnPropertyChanged(string name)
        {
            DataContext = null;
            DataContext = this;
        }
    }
}
