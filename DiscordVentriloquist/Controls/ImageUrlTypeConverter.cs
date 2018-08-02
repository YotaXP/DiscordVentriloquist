using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DiscordVentriloquist.Controls
{
    public class ImageUrlTypeConverter : IValueConverter
    {
        ImageSourceConverter converter = new ImageSourceConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as Uri)?.ToString() == "")
                return null;

            if (converter.CanConvertFrom(value?.GetType()))
                try {
                    return converter.ConvertFrom(value);
                }
                catch {}

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (converter.CanConvertTo(targetType))
                return converter.ConvertTo(value, targetType);
            else
                return null;
        }
    }
}
