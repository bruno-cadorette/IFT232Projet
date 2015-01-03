using Core.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ift232UI
{
    [ValueConversion(typeof(int), typeof(Position))]
    public class IntToPositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Position maxBound = parameter as Position;
            int x = (int)value % maxBound.X;
            int y = (int)value / maxBound.Y;
            return new Position(x, y);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var position = (Position)value;
            return position.Y * (parameter as Position).Y + position.X;
        }
    }
}
