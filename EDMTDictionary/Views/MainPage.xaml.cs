using EDMTDictionary.Helpers;
using EDMTDictionary.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EDMTDictionary
{
   
    public sealed partial class MainPage : Page
    {
        private List<Words> source = new List<Words>();
        private long lastSearchTimeInMilis = 0;
        private void SetFullScreen()
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
        }
        private void loadDictionary()
        {
            List<string> groupSource = Database.getGroupChar("Words");
            groupSource.Sort();
            lstGroup.ItemsSource = groupSource;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Handle Hardware Back Button
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            SetFullScreen();
            loadDictionary();
            MainPivot.SelectedIndex = Common.MainPivotSaveIndex;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
        }

        private async void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            if(lstAlphaBetic.Visibility == Visibility.Visible)
            {
                lstAlphaBetic.Visibility = Visibility.Collapsed;
                lstGroup.Visibility = Visibility.Visible;
            }
            else
            {
                var msg = new MessageDialog("Do you want to close this app?");
                var okBtn = new UICommand("Accept");
                var cancelBtn = new UICommand("Decline");
                msg.Commands.Add(okBtn);
                msg.Commands.Add(cancelBtn);
                IUICommand result = await msg.ShowAsync();
                if (result != null && result.Label == "Accept")
                    Application.Current.Exit();
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
        }


        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Lacking code
            if(MainPivot.SelectedIndex == Common.HomeIndex)
            {
                btnMulti.Visibility = Visibility.Collapsed;
                btnRemind.Visibility = Visibility.Collapsed;
            }
            else if(MainPivot.SelectedIndex == Common.SearchIndex)
            {
                btnMulti.Visibility = Visibility.Collapsed;
                btnRemind.Visibility = Visibility.Collapsed;
            }
            else if(MainPivot.SelectedIndex == Common.VideosIndex)
            {
                loadVideoList(); // need to implement
                btnMulti.Visibility = Visibility.Collapsed;
                btnRemind.Visibility = Visibility.Collapsed;
            }
            else if(MainPivot.SelectedIndex == Common.FavoritesIndex)
            {
                List<Words> lstsource = Database.getAllWords("Favorites");
                lstFavorites.ItemsSource = lstsource;
                if (lstFavorites.Items.Count > 0)
                    btnMulti.Visibility = Visibility.Visible;
                btnRemind.Visibility = Visibility.Visible;
                       
            }
            else if(MainPivot.SelectedIndex == Common.RecentsIndex)
            {
                List<Words> lstsource = Database.getAllWords("Recents");
                lstRecents.ItemsSource = lstsource;
                if (lstRecents.Items.Count > 0)
                    btnMulti.Visibility = Visibility.Visible;
                btnRemind.Visibility = Visibility.Visible;
            }
            else { return; }
        }

        private void loadVideoList()
        {
            List<Video> lstVideoSource = Common.getListVideo();
            lstVideo.ItemsSource = lstVideoSource;
        }

        private void lstGroup_ItemClick(object sender, ItemClickEventArgs e)
        {
            lstAlphaBetic.Visibility = Visibility.Visible;
            lstGroup.Visibility = Visibility.Collapsed;
            string groupChar = e.ClickedItem as string;
            lstAlphaBetic.ItemsSource = Database.getAllWordsByChar(groupChar);
        }

        private void lstAlphaBetic_ItemClick(object sender, ItemClickEventArgs e)
        {
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Database.insertIntoTable("Recents",word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void lstAlphaBetic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void btnSpeechRecognition_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var speechRecognizer = new SpeechRecognizer();
            await speechRecognizer.CompileConstraintsAsync();
            SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();
            txtSearch.Text = speechRecognitionResult.Text;

        }

        
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtSearch != null)
            {
                if(txtSearch.Text.Length > 0)
                {
                    if(lastSearchTimeInMilis + 500 > DateTime.Now.Millisecond)
                    {
                        lastSearchTimeInMilis = DateTime.Now.Millisecond;
                        source = Database.searchWord(txtSearch.Text.Trim());
                        if(source != null)
                        {
                            if(source.Count == 0)
                            { }
                            else
                            {
                                lstSearch.Visibility = Visibility.Visible;
                                lstSearch.ItemsSource = source;
                            }
                        }
                    }
                }
                else
                {
                    lstSearch.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void lstSearch_ItemClick(object sender, ItemClickEventArgs e)
        {
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;

        }

        private void lstVideo_ItemClick(object sender, ItemClickEventArgs e)
        {
            Video video = e.ClickedItem as Video;
            Frame.Navigate(typeof(ViewVideo), video);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void lstFavorites_ItemClick(object sender, ItemClickEventArgs e)
        {
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;

        }

        private void lstFavorites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFavorites.SelectedItems.Count > 0)
                btnDelete.IsEnabled = true;
            else
                btnDelete.IsEnabled = false;
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Words word = (e.OriginalSource as FrameworkElement).DataContext as Words;
            if(word != null)
            {
                Database.deleteFromTable("Favorites", word);
                lstFavorites.ItemsSource = Database.getAllWords("Favorites");
            }
        }

        private void lstRecents_ItemClick(object sender, ItemClickEventArgs e)
        {
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void lstRecents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRecents.SelectedItems.Count > 0)
                btnDelete.IsEnabled = true;
            else
                btnDelete.IsEnabled = false;
        }

        private void RecentsFlyoutDelete_Click(object sender, RoutedEventArgs e)
        {
            Words word = (e.OriginalSource as FrameworkElement).DataContext as Words;
            if(word != null)
            {
                Database.deleteFromTable("Recents", word);
                lstRecents.ItemsSource = Database.getAllWords("Recents");
            }
        }

        private void btnRemind_Click(object sender, RoutedEventArgs e)
        {
            RegisterBackgroundTask(1, false); // 1 = 1 hour
        }

        private async void RegisterBackgroundTask(uint hour, bool oneShot)
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            hour *= 60;
            if(result == BackgroundAccessStatus.Denied)
            {

            }

            var taskRegisted = false;
            var taskName = "ReminderWords";
            foreach(var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)
                    task.Value.Unregister(true); // Unregister task
            }

            if(taskRegisted == false)
            {
                var builder = new BackgroundTaskBuilder();
                var trigger = new TimeTrigger(hour, oneShot);

                builder.Name = taskName;
                builder.TaskEntryPoint = typeof(BackgroundTask.BackgroundTask).FullName;
                builder.SetTrigger(trigger);

                var taskRegistion = builder.Register();
                taskRegistion.Completed += TaskRegistion_Completed;
            }
        }

        private void TaskRegistion_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            
        }

        private void btnMulti_Click(object sender, RoutedEventArgs e)
        {
            int index = MainPivot.SelectedIndex;
            if(index == Common.FavoritesIndex)
            {
                if(lstFavorites.SelectionMode != ListViewSelectionMode.Multiple)
                {
                    lstFavorites.SelectionMode = ListViewSelectionMode.Multiple;
                    lstFavorites.IsItemClickEnabled = false;
                    EnableMultiSelectMode();
                }
                else
                {
                    lstFavorites.SelectionMode = ListViewSelectionMode.Single;
                    lstFavorites.IsItemClickEnabled = true;
                    DisableMultiSelectMode();
                }
            }
            else if(index == Common.RecentsIndex)
            {
                if(lstRecents.SelectionMode != ListViewSelectionMode.Multiple)
                {
                    lstRecents.SelectionMode = ListViewSelectionMode.Multiple;
                    lstRecents.IsItemClickEnabled = false;
                    EnableMultiSelectMode();
                }
                else
                {
                    lstRecents.SelectionMode = ListViewSelectionMode.Single;
                    lstRecents.IsItemClickEnabled = true;
                    DisableMultiSelectMode();
                }
            }
        }

        private void DisableMultiSelectMode()
        {
            btnMulti.Visibility = Visibility.Visible;
            btnSelectAll.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Collapsed;
        }

        private void EnableMultiSelectMode()
        {
            btnMulti.Visibility = Visibility.Collapsed;
            btnSelectAll.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
            btnDelete.IsEnabled = false;
            btnCancel.Visibility = Visibility.Visible;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(MainPivot.SelectedIndex == Common.FavoritesIndex)
            {
                if(lstFavorites.SelectedItems.Count > 0)
                {
                    foreach(Words item in lstFavorites.SelectedItems)
                    {
                        Database.deleteFromTable("Favorites", item);
                    }
                    lstFavorites.ItemsSource = Database.getAllWords("Favorites");
                }
                else
                {
                    Common.showMessage("Please select item to delete...");
                }
            }
            else if(MainPivot.SelectedIndex == Common.RecentsIndex)
            {
                if(lstRecents.SelectedItems.Count > 0)
                {
                    foreach (Words item in lstRecents.SelectedItems)
                        Database.deleteFromTable("Recents", item);
                    lstRecents.ItemsSource = Database.getAllWords("Recents");
                }
                else
                {
                    Common.showMessage("Please select item to delete...");
                }
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DisableMultiSelectMode();
            if (MainPivot.SelectedIndex == Common.FavoritesIndex)
                lstFavorites.SelectionMode = ListViewSelectionMode.Single;
            else if (MainPivot.SelectedIndex == Common.RecentsIndex)
                lstRecents.SelectionMode = ListViewSelectionMode.Single;
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (MainPivot.SelectedIndex == Common.FavoritesIndex)
                lstFavorites.SelectAll();
            else if (MainPivot.SelectedIndex == Common.RecentsIndex)
                lstRecents.SelectAll();
        }
    }
}
