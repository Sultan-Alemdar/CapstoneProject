using DesktopApp.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopApp.Core.Models
{
    public class ChannelMessageModel : BindableBase
    {
        public enum EnumMessageType
        {
            Default = 0,
            Notify = 1,
        }

        public enum EnumAboutWhat
        {
            BeSeen = 0,// görüldü görülmedi mevzusu
            Answer = 1,//answer to offer
            Waiting = 2, //waiting for new chunk
            Notify = 3,// notify about notify// belki yani her ihtimale karşı
            Null = 9
        }

        private MessageModel _messageModel = null;
        private readonly string _messageModelId = null;
        private readonly EnumMessageType _messageType = EnumMessageType.Default;

        private readonly EnumAboutWhat _aboutWhat = EnumAboutWhat.Answer;

        public MessageModel MessageModel { get => _messageModel; private set => _messageModel = value; }

        public string MessageModelId => _messageModelId;

        public EnumMessageType MessageType => _messageType;

        public EnumAboutWhat About => _aboutWhat;

        private ChannelMessageModel(MessageModel messageModel, string messageModelId, EnumMessageType messageType, EnumAboutWhat aboutWhat)
        {
            _messageModel = messageModel;
            _messageModelId = messageModelId;
            _messageType = messageType;
            _aboutWhat = aboutWhat;
        }



        public static ChannelMessageModel GetDefaultType(MessageModel messageModel)
        {
            return new ChannelMessageModel(messageModel, null, EnumMessageType.Default, EnumAboutWhat.Null);
        }

        public static ChannelMessageModel GetNotifyType(string messageModelId, EnumAboutWhat aboutWhat)
        {
            return new ChannelMessageModel(null, messageModelId, EnumMessageType.Notify, aboutWhat);
        }
    }
}
