using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DesktopApp.Helpers
{
    class ByteToBigOneConverter : IValueConverter
    {
        const double GB = 1073741824;
        const double MB = 1048576;
        const double KB = 1024;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //    ulong val;
            var val = (long)value;
            string r;
            if (val >= (long)GB)
            {
                r = (val / GB).ToString("0.00") + " GB";
            }
            else if (val >= (long)MB)
            {
                r = (val / MB).ToString("0.00") + " MB";
            }
            else if (val >= (long)KB)
            {
                r = (val / KB).ToString("0.00") + " KB";
            }
            else
                r = val.ToString("0.00") + " B";
            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
