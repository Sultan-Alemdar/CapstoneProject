using DesktopApp.Core.Models.Base;
using Newtonsoft.Json;
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
        public enum EnumEvent
        {
            Received = 0,
            Send = 1
        }
        public enum EnumSeen
        {
            No = 0,
            Yes = 1,
        }

        [JsonProperty] private EnumSeen _seen = EnumSeen.No;

        [JsonProperty] private readonly string _id; //<PID>546</PID><MID>53</MID>
        [JsonProperty] private readonly string _text = "Deneme Mesajı";      //ben kimim gibi mesala
        [JsonProperty] private readonly string _time = "13:10";      //13:14
        [JsonProperty] private readonly EnumIsExist _isFileExist = EnumIsExist.Yes;
        [JsonProperty] private readonly EnumIsExist _isTextExist = EnumIsExist.Yes;
        [JsonProperty] private FileModel _file = null;

        [JsonProperty] private EnumEvent _event;//sended received

        public string Id { get => _id; }
        public string Text { get => _text; }
        public string Time { get => _time; }
        public EnumIsExist IsFileExist { get => _isFileExist; }
        public EnumIsExist IsTextExist { get => _isTextExist; }

        public FileModel File { get => _file; set => SetProperty<FileModel>(ref this._file, value, "File"); }

        public EnumEvent Event { get => _event; set => SetProperty<EnumEvent>(ref this._event, value, "Event"); }
        public EnumSeen Seen { get => _seen; set => SetProperty<EnumSeen>(ref this._seen, value, "Seen"); }

        public MessageModel(string messageId, string time, EnumEvent eventt, string text, FileModel file = null)
        {
            _id = messageId;
            _time = time;
            if (text == "" || text == null)
            {
                _isTextExist = EnumIsExist.No;
                _text = "";

            }
            else
            {
                _text = text;
            }

            if (file == null)
                _isFileExist = EnumIsExist.No;

            _file = file;
            _event = eventt;
        }
        /// <summary>
        /// It provides visibility behavior integrity at both peer side. A peer who is a receiver, must call this method to be provided a receiver threatment to message.
        /// </summary>
        /// <param name="message"></param>
        public void SwitchTreatment()
        {

            if (this.Event == EnumEvent.Send)
            {
                this.Event = EnumEvent.Received;
                Seen = EnumSeen.Yes;
                if (this.File != null)
                    this.File.Event = FileModel.EnumEvent.Download;
            }


        }
    }
}
#region Old
//private EnumIsExist _isReceived = EnumIsExist.No;
//private EnumIsExist _isSent = EnumIsExist.Yes;
//public EnumIsExist IsReceived { get => _isReceived; private set => SetProperty<EnumIsExist>(ref this._isReceived, value, "IsReceived"); }
//public EnumIsExist IsSent
//{
//    get => _isSent;
//    set
//    {
//        SetProperty<EnumIsExist>(ref this._isSent, value, "IsSent");
//        if (value == EnumIsExist.Yes)
//        {
//            IsReceived = EnumIsExist.No;

//        }
//        else
//        {
//            IsReceived = EnumIsExist.Yes;

//        }
//    }
//} 
#endregion
