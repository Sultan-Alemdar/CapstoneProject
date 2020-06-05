using DesktopApp.Core.Models.Base;
using Newtonsoft.Json;
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
            Offer = 1,
            Accepted = 2,
            Canceled = 3,
            Start = 4,
            Continue = 8,
            End = 5,
            Error = 6,
            Waiting = 7,
            Next=10,
            Failure = 8,
            SeenOfPlainTextOrOfferMessage = 9,
        }


        [JsonProperty] private readonly string _id = null;
        [JsonProperty] private readonly string _expectedChunkId = null;
        [JsonProperty] private readonly string _expectedNextChunkId = null;
        [JsonProperty] private readonly string _errorMessage = null;
        [JsonProperty] private readonly MessageModel _messageModel = null;
        [JsonProperty] private readonly EnumMessageType _messageType;



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
            return new TreatmentMessageModel(messageModel, EnumMessageType.Offer);
        }

        public static TreatmentMessageModel GetAcceptedType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.Accepted);
        }

        public static TreatmentMessageModel GetWaitingType(string id, string expectedChunkId, string expectedNextChunkId)
        {
            return new TreatmentMessageModel(id, EnumMessageType.Waiting, expectedChunkId, expectedNextChunkId);
        }

        public static TreatmentMessageModel GetFileCanceledType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.Canceled);
        }

        public static TreatmentMessageModel GetEndType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.End);
        }
        public static TreatmentMessageModel GetContinueType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.Continue);
        }
        public static TreatmentMessageModel GetStartType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.Start);
        }

        public static TreatmentMessageModel GetNextType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.Next);
        }
        public static TreatmentMessageModel GetFailureType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.Failure);
        }
        public static TreatmentMessageModel GetErrorType(string id, string errorMessage)
        {
            return new TreatmentMessageModel(id, errorMessage, EnumMessageType.Error);
        }
        public static TreatmentMessageModel GetSeenOfPlainTextOrOfferMessageType(string id)
        {
            return new TreatmentMessageModel(id, EnumMessageType.SeenOfPlainTextOrOfferMessage);
        }
    }
}
