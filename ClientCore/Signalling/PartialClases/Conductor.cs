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
           var  _dataChannel = PeerConnection.CreateDataChannel("Deneme", DataChannelInit);
            RTCDataChannel rTCDataChannel = (RTCDataChannel)_dataChannel;
            _rTCDataChannel = rTCDataChannel;         
            _rTCDataChannel.OnOpen += _rTCDataChannel_OnOpen;
           
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
            Debug.WriteLine("Data Channel Girdi");
            IRTCDataChannel channel = e.Channel;
            Debug.WriteLine(" Data Channel Eklendi " + channel.BinaryType);
            _rTCDataChannel = (RTCDataChannel)channel;
            _rTCDataChannel.OnMessage += _rTCDataChannel_OnMessage;

        }
    }
}
