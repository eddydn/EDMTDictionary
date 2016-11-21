using EDMTDictionary.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EDMTDictionary.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {

        private void loadComboBoxItem()
        {
            List<string> lstLanguage = new List<string>();
            lstLanguage.Add("vi"); // vietnamese
            lstLanguage.Add("fr"); // France

            cbLanguageTo.ItemsSource = lstLanguage;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += Settings_BackRequested;
            loadComboBoxItem();
            loadSettingsData();
        }

        private void loadSettingsData()
        {
            //Value mean
            //See convertIntToValue in ViewWord.xaml.cs
            int rate = Convert.ToInt32(Database.getUserData(Common.SpeakRateColumn));
            int range = Convert.ToInt32(Database.getUserData(Common.SpeakRangeColumn));
            int pitch = Convert.ToInt32(Database.getUserData(Common.SpeakPitchColumn));
            int volume = Convert.ToInt32(Database.getUserData(Common.SpeakVolumeColumn));
            int male = Convert.ToInt32(Database.getUserData(Common.SpeakVoiceGender));
            int wordFontSize = Convert.ToInt32(Database.getUserData(Common.WordFontSize));
            int typeFontSize = Convert.ToInt32(Database.getUserData(Common.TypeFontSize));
            int desFontSize = Convert.ToInt32(Database.getUserData(Common.DescriptionFontSize));

            rateSlider.Value = rate;
            pitchSlider.Value = pitch;
            RangeSlider.Value = range;
            volumeSlider.Value = volume;
            wordSlider.Value = wordFontSize;
            typeSlider.Value = typeFontSize;
            desSlider.Value = desFontSize;
            cbLanguageTo.SelectedItem = Database.getUserData(Common.LanguageTo.ToString());

            if (male == 1)
                Male.IsChecked = true;
            else
                FeMale.IsChecked = true;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= Settings_BackRequested;
        }

        private  async void Settings_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            var msg = new MessageDialog("Do you want to save this changes ?");
            var okBtn = new UICommand("Accept");
            var cancelBtn = new UICommand("Decline");
            msg.Commands.Add(okBtn);
            msg.Commands.Add(cancelBtn);
            IUICommand result = await msg.ShowAsync();
            if (result != null && result.Label == "Accept")
            {
                Database.setUserData(Common.SpeakPitchColumn, Convert.ToInt32(pitchSlider.Value));
                Database.setUserData(Common.SpeakRangeColumn, Convert.ToInt32(RangeSlider.Value));
                Database.setUserData(Common.SpeakRateColumn, Convert.ToInt32(rateSlider.Value));
                Database.setUserData(Common.SpeakVolumeColumn, Convert.ToInt32(volumeSlider.Value));
                Database.setUserData(Common.WordFontSize, Convert.ToInt32(wordSlider.Value));
                Database.setUserData(Common.TypeFontSize, Convert.ToInt32(typeSlider.Value));
                Database.setUserData(Common.DescriptionFontSize, Convert.ToInt32(desSlider.Value));
                if (cbLanguageTo.SelectedIndex != -1)
                    Database.setUserData(Common.LanguageTo, cbLanguageTo.SelectedItem.ToString());

            }

            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        public Settings()
        {
            this.InitializeComponent();
        }

        private void wordSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            txtWord.FontSize = (double)(wordSlider.Value);
        }

        private void typeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            txtType.FontSize = (double)(typeSlider.Value);
        }

        private void desSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            txtDescription.FontSize = (double)(desSlider.Value);
        }

        private void Male_Checked(object sender, RoutedEventArgs e)
        {
            Database.setUserData(Common.SpeakVoiceGender, 1);
        }

        private void FeMale_Checked(object sender, RoutedEventArgs e)
        {
            Database.setUserData(Common.SpeakVoiceGender, 0);
        }
    }
}
