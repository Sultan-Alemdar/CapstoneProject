using DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRTCAdapter.Adapters;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace DesktopApp.Old.Views
{
    public sealed partial class RemoteConnectionPage : Page, INotifyPropertyChanged
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //ViewModel = (RemoteConnectionViewModel)e.Parameter;
            //this.DataContext = ViewModel;
            ViewModel.PeerVideo = PeerVideo;
            ViewModel.SelfVideo = SelfVideo;
            ViewModel.SetupScreenCapturer(this);
        }
        /// <summary>
        /// Media Failed event handler for remote/peer video.
        /// Invoked when an error occurs in peer media source.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the exception routed event.</param>
        private void PeerVideo_MediaFailed(object sender, Windows.UI.Xaml.ExceptionRoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.PeerVideo_MediaFailed(sender, e);
            }
        }

        /// <summary>
        /// Media Failed event handler for self video.
        /// Invoked when an error occurs in self media source.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the exception routed event.</param>
        private void SelfVideo_MediaFailed(object sender, Windows.UI.Xaml.ExceptionRoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.SelfVideo_MediaFailed(sender, e);
            }
        }

        /// <summary>
        /// Invoked when the mouse pointer is moved 
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the pointer routed event.</param>
        private void Page_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            ViewModel.MousePosition = e.GetCurrentPoint(this);
        }
    }
}
