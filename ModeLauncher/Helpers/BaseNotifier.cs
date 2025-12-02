using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ModeLauncher.Helpers
{
    public class BaseNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
