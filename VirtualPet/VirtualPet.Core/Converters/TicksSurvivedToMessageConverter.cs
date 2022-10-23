using System;
using System.Globalization;
using System.Windows.Data;

namespace VirtualPet.Core.Converters
{
    /// <summary>
    /// Provides a message based on the number of ticks the user has survived for.
    /// </summary>
    /// <remarks>
    /// Conversion back is not supported.
    /// </remarks>
    public class TicksSurvivedToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1:
                    return "Your pets have survived 1 tick";

                default:
                    return $"Your pets have survived {(int)value} ticks";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
