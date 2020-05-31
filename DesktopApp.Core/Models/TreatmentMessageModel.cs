using DesktopApp.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopApp.Core.Models
{
    public class TreatmentMessageModel : BindableBase
    {
        public enum EnumMessageType
        {
            PlainText = 0,
            FileOffer = 1,
            FileOfferAnswer = 2,
            FileWaiting = 3,
            FileOfferDeclined = 4,
            FileOfferCompleted = 5,
            FileOfferError = 6,
            SeenOfPlainTextOrFileOfferMessage = 7,
        }


        private readonly string _id = null;
        private readonly string _expectedChunkId = null;
        private readonly string _expectedNextChunkId = null;
        private readonly string _errorMessage = null;
        private readonly MessageModel _messageModel = null;
        private readonly EnumMessageType _messageType;



        public string Id => _id;
        public EnumMessageType MessageType => _messageType;
        public MessageModel MessageModel => _messageModel;

        public string ExpectedChunkId => _expectedChunkId;

        public string ExpectedNextChunkId => _expectedNextChunkId;

        public string ErrorMessage => _errorMessage;



        private TreatmentMessageModel(MessageModel messageModel, EnumMessageType messageType)
        {
            _messageModel = messageModel;
            _messageType = messageType;

        }

        private TreatmentMessageModel(string id, EnumMessageType messageType)
        {
            _id = id;
            _messageType = messageType;

        }

        private TreatmentMessageModel(string id, EnumMessageType messageType, string expectedChunkId, string expectedNextChunkId)
        {
            _id = id;
            _messageType = messageType;

        }
        public TreatmentMessageModel(string id, string errorMessage, EnumMessageType messageType)
        {
            _id = id;
            _errorMessage = errorMessage;
            _messageType = messageType;
        }
        public static TreatmentMessageModel GetPlainTextType(MessageModel messageModel)
        {
            return new TreatmentMessageModel(messageModel, EnumMessageType.PlainText);
        }

        public static TreatmentMessageModel GetFileOfferType(MessageModel messageModel)
        {
            return new TreatmentMessageModel(messageModel, EnumMessageType.FileOffer);
        }

        public static TreatmentMessageModel GetFileOfferAnswerType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.FileOfferAnswer);
        }

        public static TreatmentMessageModel GetFileWaitingType(string id, string expectedChunkId, string expectedNextChunkId)
        {
            return new TreatmentMessageModel(id, EnumMessageType.FileWaiting, expectedChunkId, expectedNextChunkId);
        }

        public static TreatmentMessageModel GetFileOfferDeclinedType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.FileOfferDeclined);
        }

        public static TreatmentMessageModel GetFileOfferCompletedType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.FileOfferCompleted);
        }

        public static TreatmentMessageModel GetFileOfferErrorType(string id, string errorMessage)
        {
            return new TreatmentMessageModel(id, errorMessage, EnumMessageType.FileOfferError);
        }
        public static TreatmentMessageModel GetSeenOfPlainTextOrFileOfferMessageType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.SeenOfPlainTextOrFileOfferMessage);
        }
    }
}
