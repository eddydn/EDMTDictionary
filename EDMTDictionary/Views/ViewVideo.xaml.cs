using EDMTDictionary.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MyToolkit.Multimedia;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EDMTDictionary.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewVideo : Page
    {
        Library library = new Library();
        public ViewVideo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= ViewVideo_BackRequested;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += ViewVideo_BackRequested;
            library.Init();
            library.Playing += () => {
                Position.Value = (int)Display.Position.TotalMilliseconds;
            };

            Video video = e.Parameter as Video;
            var url = await YouTube.GetVideoUriAsync(Common.getVideoIdFromUrl(video.Url), YouTubeQuality.Quality720P);
            Display.Source = url.Uri;
            Display.Play();
        }

        private void ViewVideo_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Display != null && Volume != null)
                Display.Volume = (double)Volume.Value;
        }

        private void Display_MediaOpened(object sender, RoutedEventArgs e)
        {
            Position.Maximum = (int)Display.NaturalDuration.TimeSpan.TotalMilliseconds;
            Display.Play();
            btnPlay.Icon = new SymbolIcon(Symbol.Pause);
            btnPlay.Label = "Pause";
        }

        private void Display_MediaEnded(object sender, RoutedEventArgs e)
        {
            btnPlay.Icon = new SymbolIcon(Symbol.Play);
            btnPlay.Label = "Play";
            Display.Stop();
            Position.Value = 0;

        }

        private void Display_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            library.Timer(Display.CurrentState == MediaElementState.Playing);
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(Display.CurrentState == MediaElementState.Playing)
            {
                Display.Pause();
                btnPlay.Icon = new SymbolIcon(Symbol.Play);
                btnPlay.Label = "Play";
            }
            else
            {
                Display.Play();
                btnPlay.Icon = new SymbolIcon(Symbol.Pause);
                btnPlay.Label = "Pause";
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Display.Stop();
        }
    }
}
