using System;
using System.Globalization;
using System.Windows.Data;

namespace Mypple_MusicAdminV2.Converter
{
    public class GenderToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value ?? "").ToString() == "男")
            {
                return 0;
            }
            else if ((value ?? "").ToString() == "女")
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
            {
                return "男";
            }
            else if((int)value == 1)
            {
                return "女";
            }
            else
            {
                return "武装直升机";
            }
        }
    }
}
