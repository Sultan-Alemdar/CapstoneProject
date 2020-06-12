using GalaSoft.MvvmLight;
using PeerConnectionClientOperators.Signalling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DesktopApp.ViewModels
{//For WebRTC
    public sealed partial class RemoteConnectionViewModel : ViewModelBase
    {
        private MediaElement _selfVideo;

        public MediaElement SelfVideo
        {
            get { return _selfVideo; }
            set
            {
                _selfVideo = value;
                AdapterViewModel.SelfVideo = _selfVideo;
            }
        }

        private MediaElement _peerVideo;

        public MediaElement PeerVideo
        {
            get { return _peerVideo; }
            set
            {
                _peerVideo = value;
                AdapterViewModel.PeerVideo = _peerVideo;
            }
        }
        /// <summary>
        /// Sets up a screen capturer.
        /// </summary>
        /// <param name="uiElement">Main page UI element.</param>
        public void SetupScreenCapturer(UIElement uiElement)
        {
            AdapterViewModel.SetupScreenCapturer(uiElement);
        }

        /// <summary>
        /// Media Failed event handler for the self video.
        /// Invoked when an error occurs in the self media source.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the exception routed event.</param>
        public void SelfVideo_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Debug.WriteLine("SelfVideo_MediaFailed");
        }

        /// <summary>
        /// Media Failed event handler for remote/peer video.
        /// Invoked when an error occurs in peer media source.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the exception routed event.</param>
        public void PeerVideo_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Debug.WriteLine("PeerVideo_MediaFailed");
        }

        //public Windows.UI.Input.PointerPoint MousePosition
        //{
        //  //  set { AdapterViewModel.MousePosition = value.Position; }
        //}
    }
}
