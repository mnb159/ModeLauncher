using ModeLauncher.Models;
using ModeLauncher.Services;
using ModeLauncher.ViewModels;
using System.Windows;

namespace ModeLauncher
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(LauncherConfig config, ConfigService service)
        {
            InitializeComponent();
            DataContext = new SettingsViewModel(config, service);
        }
    }
}
