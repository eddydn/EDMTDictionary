using EDMTDictionary.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
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
   
    public sealed partial class ViewWord : Page
    {
        private Words word;
        private int isFavorite;

        

        private void loadWordsDetails(Words word)
        {
            txtWord.Text = word.Word;
            txtType.Text = word.Type;
            txtDescription.Text = word.Description;
        }

        private string convertIntToValue(string type,int value)
        {
            string result = "";
            if (type.Equals("pitch"))
            {
                switch (value)
                {
                    case 1:
                        result = "x-low";
                        break;
                    case 2:
                        result = "low";
                        break;
                    case 3:
                        result = "medium";
                        break;
                    case 4:
                        result = "high";
                        break;
                    case 5:
                        result = "x-high";
                        break;
                    default:
                        result = "default";
                        break;
                }
            }
            else if (type.Equals("rate"))
            {
                switch (value)
                {
                    case 1:
                        result = "x-slow";
                        break;
                    case 2:
                        result = "slow";
                        break;
                    case 3:
                        result = "medium";
                        break;
                    case 4:
                        result = "fast";
                        break;
                    case 5:
                        result = "x-fast";
                        break;
                    default:
                        result = "default";
                        break;
                }
            }
            else if (type.Equals("volume"))
            {
                switch (value)
                {
                    case 0:
                        result = "silent";
                        break;
                    case 1:
                        result = "x-soft";
                        break;
                    case 2:
                        result = "soft";
                        break;
                    case 3:
                        result = "medium";
                        break;
                    case 4:
                        result = "loud";
                        break;
                    case 5:
                        result = "x-loud";
                        break;
                    default:
                        result = "default";
                        break;
                }
            }
            else
            {
                result = "default";
            }
            return result;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Handle HardwareBack Button
            SystemNavigationManager.GetForCurrentView().BackRequested += ViewWord_BackRequested;
            word = e.Parameter as Words;
            loadWordsDetails(word);
            isFavorite = Database.searchFavorites(word);
            //Check Favorites
            if(isFavorite == 1)
            {
                btnFavorites.Icon = new SymbolIcon(Symbol.UnFavorite);
                btnFavorites.Label = "UnFavorite";
            }
            else 
            {
                btnFavorites.Icon = new SymbolIcon(Symbol.Favorite);
                btnFavorites.Label = "Favorite";
            }
            
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= ViewWord_BackRequested;
        }

        private void ViewWord_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        public ViewWord()
        {
            this.InitializeComponent();
        }

        private async void btnSpeak_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int rate = Convert.ToInt32(Database.getUserData(Common.SpeakRateColumn));
            int range = Convert.ToInt32(Database.getUserData(Common.SpeakRangeColumn));
            int pitch = Convert.ToInt32(Database.getUserData(Common.SpeakPitchColumn));
            int volume = Convert.ToInt32(Database.getUserData(Common.SpeakVolumeColumn));
            int gender = Convert.ToInt32(Database.getUserData(Common.SpeakVoiceGender));

            MediaElement mediaPlayer = new MediaElement();
            using(var speech = new SpeechSynthesizer())
            {
                //We will use Ssml for format voice
                //You can google Ssml Document from Microsoft
                var Ssml = String.Format("<speak version='1.0' " +
                    "xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>"+
                    "<prosody rate='{0}' volume='{1}' pitch='{2}' range='{3}'>'{4}'</prosody>"
                    +
                    "</speak>", convertIntToValue("rate", rate), convertIntToValue("volume", volume), convertIntToValue("pitch", pitch), convertIntToValue("range", range), txtWord.Text);
                Debug.WriteLine(Ssml);
                if (gender == 1)
                    speech.Voice = SpeechSynthesizer.AllVoices.First(x => x.Gender == VoiceGender.Male);
                else
                    speech.Voice = SpeechSynthesizer.AllVoices.First(x => x.Gender == VoiceGender.Female);

                SpeechSynthesisStream stream = await speech.SynthesizeSsmlToStreamAsync(Ssml);
                mediaPlayer.SetSource(stream, stream.ContentType);
                mediaPlayer.Play();
                

            }
        }

        private void btnFavorites_Click(object sender, RoutedEventArgs e)
        {
            if(isFavorite == 0)
            {
                Database.insertIntoTable("Favorites", word);
                btnFavorites.Icon = new SymbolIcon(Symbol.UnFavorite);
                btnFavorites.Label = "UnFavorites";
            }
            else
            {
                Database.deleteFromTable("Favorites", word);
                btnFavorites.Icon = new SymbolIcon(Symbol.Favorite);
                btnFavorites.Label = "Favorites";
            }
        }


        private async Task TranslateWord(string originalText,string langFrom,string langTo)
        {
            //Use Microsoft Translator API on data.azure.net
            //You need register Application with Data.Azure
            //Client Id and client Secret you will get from your App on Azure when you register app


            String m_PostResponse = String.Empty;
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://datamarket.accesscontrol.windows.net/v2/OAuth2-13");
                Request.Method = "POST";
                Request.ContentType = "application/x-www-form-urlencoded";
                String postData = "grant_type=client_credentials&client_id=64de1494-5ad5-4a24-8655-b08d134a995d&client_secret=chZvuibXTDgfOrKZ7%2BARN8zv%2Fs4Er6ymliypEO%2FYYnw%3D&scope=http://api.microsofttranslator.com";
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                Stream response = await Request.GetRequestStreamAsync();
                response.Write(postBytes, 0, postBytes.Length);
                response.Dispose();
                HttpWebResponse Response = (HttpWebResponse)await Request.GetResponseAsync();
                StreamReader ResponseDataStream = new StreamReader(Response.GetResponseStream());
                m_PostResponse = await ResponseDataStream.ReadToEndAsync();

                var data = m_PostResponse.Split(',');
                var accesstoken = data[1].Split(':');
                String accessTokenString = accesstoken[1].Replace("\"", String.Empty);

                Request = (HttpWebRequest)WebRequest.Create("https://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + WebUtility.UrlEncode(originalText) + "&from=" + langFrom + "&to=" + langTo);
                Request.Method = "GET";
                Request.Headers["Authorization"] = "Bearer " + accessTokenString;
                Response = (HttpWebResponse)await Request.GetResponseAsync();
                ResponseDataStream = new StreamReader(Response.GetResponseStream());
                m_PostResponse = await ResponseDataStream.ReadToEndAsync();
                XDocument doc = XDocument.Parse(m_PostResponse);
                m_PostResponse = WebUtility.UrlDecode(doc.Root.Value);

                if(m_PostResponse != null)
                {
                    Common.showMessage(m_PostResponse); // Translated Word
                }
                else
                {
                    Common.showMessage("Error");
                }
            }
            catch(Exception ex)
            {
                Common.showMessage(ex.Message);
                return;
            }
        } 
        private async void btnTranslate_Click(object sender, RoutedEventArgs e)
        {
            await TranslateWord(txtWord.Text, "en", Database.getUserData("LanguageTo").ToString());
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }
    }
}
