using ModeLauncher.Helpers;
using ModeLauncher.Models;
using ModeLauncher.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace ModeLauncher.ViewModels
{
    public class SettingsViewModel : BaseNotifier
    {
        private readonly ConfigService _configService;

        public LauncherConfig Config { get; }
        public ObservableCollection<LaunchMode> Modes { get; }

        public ICommand AddModeCommand { get; }
        public ICommand RemoveModeCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }

        private LaunchMode? _selectedMode;
        public LaunchMode? SelectedMode
        {
            get => _selectedMode;
            set { _selectedMode = value; OnPropertyChanged(); }
        }

        public SettingsViewModel(LauncherConfig config, ConfigService configService)
        {
            _configService = configService;
            Config = config;

            Modes = new ObservableCollection<LaunchMode>(config.Modes);

            AddModeCommand = new RelayCommand(_ => AddMode());
            RemoveModeCommand = new RelayCommand(_ => RemoveMode());
            SaveCommand = new RelayCommand(_ => Save());
            CloseCommand = new RelayCommand(_ => Close());
        }

        private void AddMode()
        {
            var mode = new LaunchMode
            {
                Label = "New Mode",
                Subtitle = "",
                ExecutablePath = "",
                Arguments = ""
            };

            Modes.Add(mode);
            SelectedMode = mode;
        }

        private void RemoveMode()
        {
            if (SelectedMode == null)
                return;

            Modes.Remove(SelectedMode);

            // Keep at least one mode
            if (Modes.Count == 0)
            {
                var placeholder = new LaunchMode
                {
                    Label = "New Mode",
                    Subtitle = "",
                    ExecutablePath = "",
                    Arguments = ""
                };

                Modes.Add(placeholder);
                SelectedMode = placeholder;
            }
            else
            {
                SelectedMode = Modes.First();
            }
        }

        private void Save()
        {
            Config.Modes = Modes.ToList();
            _configService.Save(Config);

            LauncherWindow.ReloadIconsRequested = true;
        }

        private void Close()
        {
            var win = System.Windows.Application.Current.Windows
                .OfType<SettingsWindow>()
                .FirstOrDefault();

            win?.Close();
        }
    }
}
