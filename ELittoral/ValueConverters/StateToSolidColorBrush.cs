using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ELittoral.ValueConverters
{
    public class StateToSolidColorBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                string formatString = value as string;
                if (!string.IsNullOrEmpty(formatString))
                {
                    if (formatString.Equals("pending"))
                    {
                        return new SolidColorBrush(Windows.UI.Colors.LightGray);
                    }
                    else if (formatString.Equals("progress"))
                    {
                        return new SolidColorBrush(Windows.UI.Colors.Blue);
                    }
                    else if (formatString.Equals("error"))
                    {
                        return new SolidColorBrush(Windows.UI.Colors.Red);
                    }
                    else if (formatString.Equals("complete"))
                    {
                        return new SolidColorBrush(Windows.UI.Colors.Green);
                    }
                }
            }

            return new SolidColorBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
