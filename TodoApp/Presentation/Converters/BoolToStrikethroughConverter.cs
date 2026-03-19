using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TodoApp.Presentation.Converters
{
    /// <summary>
    /// bool → TextDecorations 변환.
    /// 완료된 할일의 제목에 취소선 표시용.
    /// </summary>
    public class BoolToStrikethroughConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted && isCompleted)
                return TextDecorations.Strikethrough;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
