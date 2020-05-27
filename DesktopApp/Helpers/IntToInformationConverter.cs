using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DesktopApp.Helpers
{
    public class IntToInformationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            int num = (int)value;
            switch (num)
            {
                case -1:
                    return "Id could not being gotten.";

                case 0:
                    return null;

                default:
                    return num.ToString();
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (!int.TryParse((string)value, out int result))
                {
                    return 0;
                }

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("[Error] IntToInformationConverter : Trying to converting of the value to a interger ended with error. Default value will be returned : " + e.Message);
                return 0;
            }
        }
    }
}
