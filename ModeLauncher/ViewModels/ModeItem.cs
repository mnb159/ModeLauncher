using ModeLauncher.Helpers;
using System.Windows.Media;

namespace ModeLauncher.ViewModels
{
    public class ModeItem : BaseNotifier
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public string Id { get; set; } = "";
        public string Label { get; set; } = "";
        public string? Subtitle { get; set; }
        public string ExecutablePath { get; set; } = "";
        public string? Arguments { get; set; }

        private ImageSource? _icon;
        public ImageSource? Icon
        {
            get => _icon;
            set { _icon = value; OnPropertyChanged(); }
        }
    }
}
