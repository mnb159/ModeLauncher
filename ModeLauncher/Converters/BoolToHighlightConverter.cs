using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ModeLauncher.Converters
{
    public class BoolToHighlightConverter : IValueConverter
    {
        private static readonly SolidColorBrush HighlightBrush = new SolidColorBrush(Color.FromRgb(58, 181, 166));
        private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool selected = value is bool b && b;
            return selected ? HighlightBrush : TransparentBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
