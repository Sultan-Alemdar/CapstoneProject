using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using DesktopApp.ViewModels;
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

        private bool running = false;

        public event PropertyChangedEventHandler PropertyChanged;

        private RemoteConnectionViewModel ViewModel
        {
            get { return ViewModelLocator.Current.RemoteConnectionViewModel; }
        }

        public Visibility CloseButonVisibility { get => closeButonVisibility; set { closeButonVisibility = value; OnPropertyChanged("CloseButonVisibility"); } }
        public Visibility OpenButtonVisibility { get => openButtonVisibility; set { openButtonVisibility = value; OnPropertyChanged("OpenButtonVisibility"); } }


        public RemoteConnectionPage()
        {
            InitializeComponent();
            //ViewModelLocator.Current.RemoteConnectionViewModel.PastInteractionStackPanel = PastInteractionStackPanel;
            AppBarClose.Completed += AppBarClose_Completed;
            AppBarOpen.Completed += AppBarClose_Completed;
            
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AppBarClose_Completed(object sender, object e)
        {
            running = false;
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
        private void OpenButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (running)
            {
                running = false;
                AppBarClose.Stop();
                Debug.WriteLine("Close.Stop");
            }
            else
            {
                running = true;
                AppBarOpen.Begin();
                Debug.WriteLine("Open.Start");
            }

            SwichVisibility();
        }
        private void CloseButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (running)
            {
                running = false;
                AppBarOpen.Stop();
                Debug.WriteLine("Open.Stop");
            }
            else
            {
                running = true;
                AppBarClose.Begin();
                Debug.WriteLine("Close.Start");
            }

            SwichVisibility();
        }

        private void SwichVisibility()
        {

            if (openButtonVisibility == Visibility.Collapsed)
            {
                CloseButonVisibility = Visibility.Collapsed;
                OpenButtonVisibility = Visibility.Visible;
            }
            else
            {
                OpenButtonVisibility = Visibility.Collapsed;
                CloseButonVisibility = Visibility.Visible;
            }
        }
        private void AppBarStoryBoard_Completed(object sender, object e)
        {
            Debug.WriteLine("Trans X : " + Translate.X);
        }

    }
}
