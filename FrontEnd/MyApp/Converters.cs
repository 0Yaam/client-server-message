using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Avalonia.Layout;
using Avalonia.Media;

namespace MyApp.Converters
{
    // Căn trái/phải dựa vào người gửi
    public class MessageAlignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool laNguoiDung = (bool)value;
            return laNguoiDung ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    // Màu khung tin nhắn
    public class MessageColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool laNguoiDung = (bool)value;
            return new SolidColorBrush(laNguoiDung ? Color.Parse("#4a90e2") : Color.Parse("#3a3a3a"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}