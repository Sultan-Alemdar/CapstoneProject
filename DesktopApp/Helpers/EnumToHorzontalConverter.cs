using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace DesktopApp.Helpers
{
    public class EnumToHorzontalConverter : IValueConverter
    {
        public Type EnumType { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (Enum.IsDefined(EnumType, value))
            {
                var a = Enum.GetValues(EnumType);

            }
            return new object();

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
