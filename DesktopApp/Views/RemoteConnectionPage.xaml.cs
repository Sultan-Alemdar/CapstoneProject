using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using System.Threading.Tasks;
using DesktopApp.ViewModels;
using Windows.Foundation;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace DesktopApp.Views
{
    public sealed partial class RemoteConnectionPage : Page, INotifyPropertyChanged
    {
        private Visibility closeButonVisibility = Visibility.Collapsed;
        private Visibility openButtonVisibility = Visibility.Visible;
        private bool pinned = false;

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
            //AppBarClose?.Begin();
            Loaded += RemoteConnectionPage_Loaded;    
        }

        private void RemoteConnectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            AppBarClose.Begin();
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
            pinned = true;
            SwichVisibility();

        }
        private void UnpinButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            pinned = false;
            SwichVisibility();
        }

        private void AppBarGrid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            #region eski kod
            //Debug.WriteLine("Ent");
            //if (pinned == false)
            //{
            //    if (DecideInOrOut(AppBarGrid) ==true && isOpen==false)
            //    {
            //        AppBarClose.Pause();
            //        isOpen = true;
            //        AppBarOpen.Begin();
            //        Debug.WriteLine("Opened");
            //    }
            //    else if(DecideInOrOut(AppBarGrid) ==false && isOpen==true)
            //    {
            //        AppBarOpen.Pause();
            //        isOpen = false;
            //        AppBarClose.Begin();
            //        Debug.WriteLine("Closed");
            //    }
            //} 
            #endregion
            if (pinned == false)
                AppBarOpen.Begin();
        }
        private void AppBarGrid_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            #region eski kod
            //Debug.WriteLine("Ext");
            //if (pinned == false)
            //{
            //    if (DecideInOrOut(AppBarGrid) ==true && isOpen==false)
            //    {
            //        AppBarClose.Pause();
            //        isOpen = true;
            //        AppBarOpen.Begin();
            //        Debug.WriteLine("Opened");
            //    }
            //    else if(DecideInOrOut(AppBarGrid) ==false && isOpen==true)
            //    {
            //        AppBarOpen.Pause();
            //        isOpen = false;
            //        AppBarClose.Begin();
            //        Debug.WriteLine("Closed");
            //    }
            //} 
            #endregion
            if (pinned == false)
                AppBarClose.Begin();
        }

        private bool DecideInOrOut(Grid element, bool writeToOut = false, double bufferPxs = 0)
        {
            writeToOut = true;
            var pointerPosition = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
            var wp_x = pointerPosition.X - Window.Current.Bounds.X;
            var wp_y = pointerPosition.Y - Window.Current.Bounds.Y;
            var ttv = element.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var x1 = screenCoords.X - bufferPxs;
            var y1 = screenCoords.Y - bufferPxs;
            var x2 = x1 + element.ActualWidth + bufferPxs;
            var y2 = y1 + element.ActualHeight + bufferPxs;
            #region WriteToDebug
            if (writeToOut)
            {
                Debug.WriteLine("Pointer.x :" + pointerPosition.X);
                Debug.WriteLine("Pointer.y :" + pointerPosition.Y);
                Debug.WriteLine("Pointer.x on window :" + wp_x);
                Debug.WriteLine("Pointer.y on window :" + wp_y);
                Debug.WriteLine("screenCoords.x1 :" + x1);
                Debug.WriteLine("screenCoords.y1 :" + y1);
                Debug.WriteLine("screenCoords.x2 :" + x2);
                Debug.WriteLine("screenCoords.y2 :" + y2);
            }
            #endregion

            if ((wp_x >= x1 && wp_x <= x2) && (wp_y >= y1 && wp_y <= y2))
            {
                //Debug.WriteLine("Boooooom");
                return true;
            }
            return false;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MessengerPanel_LayoutUpdated(object sender, object e)
        {
            Debug.WriteLine("Boooooom");
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    List<string> attrNames = new List<string>();//({ "DeviceFamily", "OSVersionFull", "FlightRing" });
        //    attrNames.Add("DeviceFamily");
        //    attrNames.Add("OSVersionFull");
        //    attrNames.Add("FlightRing");
        //    var attrData = AnalyticsInfo.GetSystemPropertiesAsync(attrNames).AsTask().GetAwaiter().GetResult();
        //    foreach (var item in attrData)
        //    {
        //        Debug.WriteLine(item);
        //    }
        //}
    }
}
