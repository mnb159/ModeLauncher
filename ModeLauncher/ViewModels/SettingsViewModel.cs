using ModeLauncher.Helpers;
using ModeLauncher.Models;
using ModeLauncher.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
            if (SelectedMode == null) return;
            Modes.Remove(SelectedMode);
            SelectedMode = Modes.FirstOrDefault();
        }

        private void Save()
        {
            Config.Modes = Modes.ToList();
            _configService.Save(Config);
        }

        private void Close()
        {
            var window = System.Windows.Application.Current
                .Windows
                .OfType<SettingsWindow>()
                .FirstOrDefault();

            window?.Close();
        }
    }
}
