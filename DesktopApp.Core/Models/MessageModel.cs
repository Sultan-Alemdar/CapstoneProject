using DesktopApp.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopApp.Core.Models
{
    public class MessageModel : BindableBase
    {

        public enum EnumIsExist
        {
            No = 0,
            Yes = 1,
        }

        private string _text = "Deneme Mesajı";      //ben kimim gibi mesala
        private string _time = "13:10";      //13:14
        private EnumIsExist _isFileExist = EnumIsExist.Yes;
        private EnumIsExist _isTextExist = EnumIsExist.Yes;
        private FileModel _file = new FileModel();
        private EnumIsExist _isReceived = EnumIsExist.No;
        private EnumIsExist _isSent = EnumIsExist.Yes;
  



        public string Text { get => _text; set => SetProperty<string>(ref this._text, value, "Text"); }
        public string Time { get => _time; set => SetProperty<string>(ref this._time, value, "Time"); }
        public EnumIsExist IsFileExist { get => _isFileExist; set => SetProperty<EnumIsExist>(ref this._isFileExist, value, "IsFileExist"); }
        public EnumIsExist IsTextExist { get => _isTextExist; set => SetProperty<EnumIsExist>(ref this._isTextExist, value, "IsTextExist"); }

        public FileModel File { get => _file; set => SetProperty<FileModel>(ref this._file, value, "File"); }
        public EnumIsExist IsReceived { get => _isReceived; private set => SetProperty<EnumIsExist>(ref this._isReceived, value, "IsReceived"); }
        public EnumIsExist IsSent
        {
            get => _isSent;
            set
            {
                SetProperty<EnumIsExist>(ref this._isSent, value, "IsSent");
                if (value == EnumIsExist.Yes)
                {
                    IsReceived = EnumIsExist.No;
                   
                }
                else
                {
                    IsReceived = EnumIsExist.Yes;
            
                }
            }
        }

       
    }
}
