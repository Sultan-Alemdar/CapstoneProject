using DesktopApp.Constants;
using DesktopApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DesktopApp.Helpers
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int val = (int)value;
            string par = parameter as string;
            if (par == "Button")
            {

                if (val == (int)MessageModel.EnumEvent.Received)
                    return MyConstants.PLAIN_RECEIVED_COLOR;
                else
                    return MyConstants.PLAIN_SENDED_COLOR;
            }
            else
            {

                switch (val)
                {
                    case (int)FileModel.EnumProccesStatus.Waiting:
                        if (par == "Send")
                            return MyConstants.PLAIN_SENDED_COLOR;
                        else
                            return MyConstants.PLAIN_RECEIVED_COLOR;
                    case (int)FileModel.EnumProccesStatus.InQueue:
                        return MyConstants.INQUEUE_FILE_COLOR;
                    case (int)FileModel.EnumProccesStatus.Progressing:
                        return MyConstants.PROGRESSİNG_FILE_COLOR;
                    case (int)FileModel.EnumProccesStatus.Completed:
                        return MyConstants.COMPLETED_FILE_COLOR;
                    case (int)FileModel.EnumProccesStatus.Declined:
                        return "Transparent";
                }
            }
            return "Transparent";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
