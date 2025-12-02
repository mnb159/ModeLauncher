using ModeLauncher.Models;
using ModeLauncher.Services;
using ModeLauncher.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ModeLauncher
{
    public partial class LauncherWindow : Window, System.ComponentModel.INotifyPropertyChanged
    {
        public static bool ReloadIconsRequested = false;

        private readonly ConfigService _configService = new ConfigService();
        private readonly IconService _iconService = new IconService();

        private LauncherConfig _config;

        public List<ModeItem> Modes { get; private set; } = new();

        public ModeItem SelectedMode => Modes.First(m => m.IsSelected);

        private DispatcherTimer _timer;
        private int _countdown;

        public string CountdownText =>
            _countdown > 0
            ? $"Launching {SelectedMode.Label} in {_countdown}..."
            : "";

        public LauncherWindow()
        {
            InitializeComponent();

            LoadConfig();
            LoadModes();
            SetupCountdown();

            DataContext = this;
            Root.Focus();
        }

        private void LoadConfig()
        {
            _config = _configService.Load();
        }

        public void LoadModes()
        {
            Modes = new List<ModeItem>();

            foreach (var m in _config.Modes)
            {
                Modes.Add(new ModeItem
                {
                    Id = m.Id,
                    Label = m.Label,
                    Subtitle = m.Subtitle,
                    ExecutablePath = m.ExecutablePath,
                    Arguments = m.Arguments,
                    Icon = _iconService.GetIcon(m.ExecutablePath),
                    IsSelected = false
                });
            }

            var defaultMode = Modes.FirstOrDefault(m => m.Id == _config.DefaultModeId)
                              ?? Modes.First();

            defaultMode.IsSelected = true;

            OnPropertyChanged(nameof(Modes));
            OnPropertyChanged(nameof(SelectedMode));
            OnPropertyChanged(nameof(CountdownText));
        }

        private void SetupCountdown()
        {
            _countdown = _config.CountdownSeconds;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) =>
            {
                if (_countdown <= 0)
                {
                    _timer.Stop();
                    LaunchSelectedMode();
                }
                else
                {
                    _countdown--;
                    OnPropertyChanged(nameof(CountdownText));
                }
            };

            _timer.Start();
            OnPropertyChanged(nameof(CountdownText));
        }

        private void LaunchSelectedMode()
        {
            try
            {
                var m = SelectedMode;

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = m.ExecutablePath,
                    Arguments = m.Arguments ?? "",
                    UseShellExecute = true
                });

                Application.Current.Shutdown();
            }
            catch
            {
                MessageBox.Show("Unable to start the selected mode.");
            }
        }

        private void MoveSelection(int direction)
        {
            var index = Modes.IndexOf(SelectedMode);
            int newIndex = index + direction;

            if (newIndex < 0 || newIndex >= Modes.Count)
                return;

            Modes[index].IsSelected = false;
            Modes[newIndex].IsSelected = true;

            OnPropertyChanged(nameof(Modes));
            OnPropertyChanged(nameof(SelectedMode));
            OnPropertyChanged(nameof(CountdownText));
        }

        private void CancelCountdown()
        {
            _timer?.Stop();
            _countdown = 0;
            OnPropertyChanged(nameof(CountdownText));
        }

        private void Root_KeyDown(object sender, KeyEventArgs e)
        {
            CancelCountdown();

            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
                return;
            }

            if (e.Key == Key.Left)
                MoveSelection(-1);

            if (e.Key == Key.Right)
                MoveSelection(1);

            if (e.Key == Key.Enter)
                LaunchSelectedMode();

            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
                OpenSettings();
        }

        private void Root_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CancelCountdown();
        }

        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            Root.Focus();
        }

        private void ModeTile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CancelCountdown();

            if (sender is FrameworkElement fe && fe.DataContext is ModeItem clicked)
            {
                foreach (var m in Modes)
                    m.IsSelected = false;

                clicked.IsSelected = true;

                OnPropertyChanged(nameof(Modes));
                OnPropertyChanged(nameof(SelectedMode));
                OnPropertyChanged(nameof(CountdownText));
            }
        }

        private void OpenSettings()
        {
            CancelCountdown();

            var win = new SettingsWindow(_config, _configService)
            {
                Owner = this
            };

            win.ShowDialog();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (ReloadIconsRequested)
            {
                ReloadIconsRequested = false;

                LoadConfig();
                LoadModes();
                SetupCountdown();
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,
                new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
