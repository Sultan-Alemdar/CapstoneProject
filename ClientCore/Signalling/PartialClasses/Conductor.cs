using Org.WebRtc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerConnectionClientOperators.Signalling
{
    /// <summary>
    /// A singleton conductor for WebRTC session.
    /// </summary>
    public partial class Conductor
    {
        public RTCDataChannel _rTCDataChannel;
        public void CreateDataChannel()
        {
            RTCDataChannelInit DataChannelInit = new RTCDataChannelInit();
            var _dataChannel = PeerConnection.CreateDataChannel("Deneme", DataChannelInit);
            RTCDataChannel rTCDataChannel = (RTCDataChannel)_dataChannel;
            _rTCDataChannel = rTCDataChannel;
            //     _rTCDataChannel.OnMessage += _rTCDataChannel_OnMessage;
            _rTCDataChannel.OnOpen += _rTCDataChannel_OnOpen;
            _rTCDataChannel.OnError += _rTCDataChannel_OnError;
            _rTCDataChannel.OnClose += _rTCDataChannel_OnClose;
            
        }

        private void _rTCDataChannel_OnClose()
        {
            throw new NotImplementedException();
        }

        private void _rTCDataChannel_OnError(IRTCError Event)
        {
            throw new NotImplementedException();
        }

        private void _rTCDataChannel_OnMessage(IMessageEvent Event)
        {
            Debug.WriteLine("[DataChannel] Conductor: It has passed message:", Event.Text);
        }

        private void _rTCDataChannel_OnOpen()
        {
            Debug.WriteLine("[DataChannel] Conductor: It has being opened up:");
        }

        private void _peerConnection_OnDataChannel(IRTCDataChannelEvent e)
        {
            var asd = RTCDataChannelEvent.Cast(e);
            Debug.WriteLine("[Conductor] OnDataChannel Girdi. ");
            IRTCDataChannel channel = e.Channel;
            Debug.WriteLine("[Conductor] Data Channel Eklendi " + channel.BinaryType);
            _rTCDataChannel = (RTCDataChannel)channel;
            _rTCDataChannel.OnMessage += _rTCDataChannel_OnMessage;

        }
        public void SendMessage(string msg = "Test Message")
        {
 
            var asd =_rTCDataChannel.ReadyState;

            _rTCDataChannel.Send(msg);
        }
    }
}
