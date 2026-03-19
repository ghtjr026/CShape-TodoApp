using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TodoApp.Domain.Enums;

namespace TodoApp.Presentation.Converters
{
    /// <summary>
    /// TodoPriority → Brush 변환.
    /// High=Red, Medium=Orange, Low=Gray.
    /// </summary>
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TodoPriority priority)
            {
                switch (priority)
                {
                    case TodoPriority.High:
                        return Brushes.Red;
                    case TodoPriority.Medium:
                        return Brushes.Orange;
                    case TodoPriority.Low:
                        return Brushes.Gray;
                }
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
