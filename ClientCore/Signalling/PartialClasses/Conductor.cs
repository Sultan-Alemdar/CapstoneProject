using Org.WebRtc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerConnectionClientOperators.Signalling
{

    public partial class Conductor
    {
        private RTCDataChannel _messageChannel;
        private RTCDataChannel _fileChannel;
        public RTCDataChannel MessageChannel { get => _messageChannel; }
        public RTCDataChannel FileChannel { get => _fileChannel; }

        private const string FILE_CHANNEL = "FileChannel", MESSAGE_CHANNEL = "MessageChannel";


        public void CreateDataChannels()
        {
            try
            {

                RTCDataChannelInit DataChannelInit2 = new RTCDataChannelInit
                {
                    Negotiated = false,
                    Id = 2,
                    Ordered = true
                };
                var _dataChannel2 = PeerConnection.CreateDataChannel(MESSAGE_CHANNEL, DataChannelInit2);
                _messageChannel = (RTCDataChannel)_dataChannel2;

                _messageChannel.OnOpen += _messageChannel_OnOpen; ;
                _messageChannel.OnError += _messageChannel_OnError; ;
                _messageChannel.OnClose += _messageChannel_OnClose; ; ;



                RTCDataChannelInit DataChannelInit = new RTCDataChannelInit
                {
                    Negotiated = false,
                    Id = 1,
                    Ordered = true
                };
                var _dataChannel = PeerConnection.CreateDataChannel(FILE_CHANNEL, DataChannelInit);
                _fileChannel = (RTCDataChannel)_dataChannel;

                _fileChannel.OnOpen += _fileChannel_OnOpen;
                _fileChannel.OnError += _fileChannel_OnError;
                _fileChannel.OnClose += _fileChannel_OnClose;




            }
            catch (Exception e)
            {
                Debug.WriteLine("[Error] Conductor : Error was occured at data channel creation proccess : " + e.Message);

            }
        }



        private void _peerConnection_OnDataChannel(IRTCDataChannelEvent e)
        {
            RTCDataChannelEvent channelEvent = RTCDataChannelEvent.Cast(e);
            IRTCDataChannel channel = e.Channel;
            Debug.WriteLine("[Info] Conductor : New channel was added : " + channel.Label);

            if (channel.Label == MESSAGE_CHANNEL)
            {
                _messageChannel = (RTCDataChannel)channel;

            }
            else
            {
                _fileChannel = (RTCDataChannel)channel;

            }
            Debug.WriteLine("[Conductor] Data Channel Eklendi " + channel.BinaryType);

        }

        public void SendMessage(string jsonString)
        {
            if (_messageChannel.ReadyState == RTCDataChannelState.Open)
                _fileChannel.Send(jsonString);

        }




        private void _messageChannel_OnClose()
        {
            Debug.WriteLine("[DataChannel] Conductor: Message channel was closed.");
        }
        private void _fileChannel_OnClose()
        {
            Debug.WriteLine("[DataChannel] Conductor: File channel was closed.");
        }


        private void _fileChannel_OnError(IRTCError Event)
        {
            Debug.WriteLine("[Error] Conductor: Opening of file channel was ended with error:" + Event.Message);
        }
        private void _messageChannel_OnError(IRTCError Event)
        {
            Debug.WriteLine("[Error] Conductor: Opening of message channel was ended with error:" + Event.Message);
        }


        private void _messageChannel_OnOpen()
        {
            Debug.WriteLine("[DataChannel] Conductor: Message channel was opened.");
        }
        private void _fileChannel_OnOpen()
        {
            Debug.WriteLine("[DataChannel] Conductor: File channel was opened.");
        }


    }

}
