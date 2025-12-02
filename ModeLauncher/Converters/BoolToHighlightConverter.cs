using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ModeLauncher.Converters
{
    public class BoolToHighlightConverter : IValueConverter
    {
        // value: IsSelected (bool)
        // target: Brush for BorderBrush
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = value is bool b && b;

            // Selected → accent color, Unselected → subtle border
            return isSelected
                ? new SolidColorBrush(Color.FromRgb(0x4C, 0xA3, 0xFF))  // light blue
                : new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)); // dark gray
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
