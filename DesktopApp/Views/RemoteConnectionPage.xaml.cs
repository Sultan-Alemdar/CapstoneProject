using System;
using System.ComponentModel;
using System.Diagnostics;

using System.Threading.Tasks;
using DesktopApp.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace DesktopApp.Views
{
    public sealed partial class RemoteConnectionPage : Page, INotifyPropertyChanged
    {

        private const bool RUNNING = true, NOT_RUNNING = false;
        private Visibility closeButonVisibility = Visibility.Visible;
        private Visibility openButtonVisibility = Visibility.Collapsed;
        private bool pinned = false;
        private bool isInside = false;
        public event PropertyChangedEventHandler PropertyChanged;

        private RemoteConnectionViewModel ViewModel
        {
            get { return ViewModelLocator.Current.RemoteConnectionViewModel; }
        }

        public Visibility UnPinButtonVisibility { get => closeButonVisibility; set { closeButonVisibility = value; OnPropertyChanged("UnPinButtonVisibility"); } }
        public Visibility PinButtonVisibility { get => openButtonVisibility; set { openButtonVisibility = value; OnPropertyChanged("PinButtonVisibility"); } }


        public RemoteConnectionPage()
        {
            InitializeComponent();
            //ViewModelLocator.Current.RemoteConnectionViewModel.PastInteractionStackPanel = PastInteractionStackPanel;

        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ScrollViewer_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Manipualitaon Worked");

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
        private void PinButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SwichVisibility();
            pinned = true;
        }
        private void UnpinButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            pinned = false;
            SwichVisibility();
        }

        private void Border_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //Debug.WriteLine("ENt");
            //if (pinned == false)
            //{
            //    if (isInside)
            //        AppBarOpen.Begin();
            //    else
            //    {
            //        isInside = false;
            //        AppBarClose.Begin();
            //    }

            //}
        }

        private void Border_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }


        private void Border_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Exit");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var pointerPosition = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
            var wp_x = pointerPosition.X - Window.Current.Bounds.X;
            var wp_y = pointerPosition.Y - Window.Current.Bounds.Y;
            var ttv = AppBarGrid.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var x1 = screenCoords.X;
            var y1 = screenCoords.Y;
            var x2 = x1 + AppBarGrid.ActualWidth;
            var y2= y1 + AppBarGrid.ActualHeight;
            Debug.WriteLine("Pointer.x :"+pointerPosition.X);
            Debug.WriteLine("Pointer.y :" + pointerPosition.Y);
            Debug.WriteLine("Pointer.x on window :" + wp_x);
            Debug.WriteLine("Pointer.y on window :" + wp_y);
            Debug.WriteLine("screenCoords.x1 :" + x1);
            Debug.WriteLine("screenCoords.y1 :" + y1);
            Debug.WriteLine("screenCoords.x2 :" + x2);
            Debug.WriteLine("screenCoords.y2 :" + y2);
            if ( (wp_x >= x1 && wp_x <= x2) && (wp_y >= y1 && wp_y <= y2))
            {

                Debug.WriteLine("Boooooom");
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SwichVisibility()
        {
            if (openButtonVisibility == Visibility.Collapsed)
            {
                UnPinButtonVisibility = Visibility.Collapsed;
                PinButtonVisibility = Visibility.Visible;
            }
            else
            {
                PinButtonVisibility = Visibility.Collapsed;
                UnPinButtonVisibility = Visibility.Visible;
            }
        }

    }
}
