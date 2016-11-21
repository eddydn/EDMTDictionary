using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace EDMTDictionary.Helpers
{
    public class Common
    {
        //Name of our DB
        public static string DB_NAME = "MyDictionary.db";
        //Define column name in SQLite DB
        public static string SpeakRateColumn = "SpeakRate";
        public static string SpeakPitchColumn = "SpeakPitch";
        public static string SpeakRangeColumn = "SpeakRange";
        public static string SpeakVolumeColumn = "SpeakVolume";
        public static string SpeakVoiceGender = "VoiceGender";
        public static string WordFontSize = "WordFontSize";
        public static string TypeFontSize = "TypeFontSize";
        public static string DescriptionFontSize = "DescriptionFontSize";
        public static string LanguageTo = "LanguageTo";

        //Define for Pivot index
        public static int HomeIndex = 0;
        public static int SearchIndex = 1;
        public static int VideosIndex = 2;
        public static int FavoritesIndex = 3;
        public static int RecentsIndex = 4;
        public static int MainPivotSaveIndex = 0;

        //Function show message dialog
        public static async void showMessage(string errorMessage)
        {
            var msg = new MessageDialog(errorMessage);
            var okBtn = new UICommand("OK");
            msg.Commands.Add(okBtn);
            try
            {
                IUICommand result = await msg.ShowAsync();
            }
            catch
            {
                return;
            }
        }

        //Ulities for Youtube function
        public static List<Video> getListVideo()
        {
            List<Video> videoList = new List<Video>();
            Video video = new Video()
            {
                VideoTitle = "Extra English episode 1 Hectors arrival (part 1)",
                DateAdded = DateTime.Now.ToString("dd-MM-yyyy"),
                ImgUrl = getLinkImgFromUrl("https://youtu.be/k89GF-i_Eyg?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF"),
                Url = getLinkVideoFromUrl("https://youtu.be/k89GF-i_Eyg?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF")
            };
            videoList.Add(video);

            video = new Video()
            {
                VideoTitle = "Extra English episode 1 Hectors arrival (part 1)",
                Url = getLinkVideoFromUrl("https://youtu.be/mGCpd2908Ys?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF"),
                DateAdded = DateTime.Now.ToString("dd-MM-yyyy"),
                ImgUrl = getLinkImgFromUrl("https://www.youtube.com/watch?v=mGCpd2908Ys")
            };
            videoList.Add(video);

            video = new Video()
            {
                VideoTitle = "Extra English episode 2 Hectors goes shopping (part 1)",
                Url = getLinkVideoFromUrl("https://youtu.be/S060IB-OvL4?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF"),
                DateAdded = DateTime.Now.ToString("dd-MM-yyyy"),
                ImgUrl = getLinkImgFromUrl("https://youtu.be/S060IB-OvL4?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF")
            };

            videoList.Add(video);

            video = new Video()
            {
                VideoTitle = "Extra English episode 2 Hectors goes shopping (part 2)",
                Url = getLinkVideoFromUrl("https://youtu.be/j3aT65UFBUs?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF"),
                DateAdded = DateTime.Now.ToString("dd-MM-yyyy"),
                ImgUrl = getLinkImgFromUrl("https://youtu.be/j3aT65UFBUs?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF")
            };

            videoList.Add(video);

            video = new Video()
            {
                VideoTitle = "Extra English Episode 3 Hector has a date (part1)",
                Url = getLinkVideoFromUrl("https://youtu.be/5IJ0Br-wABI?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF"),
                DateAdded = DateTime.Now.ToString("dd-MM-yyyy"),
                ImgUrl = getLinkImgFromUrl("https://youtu.be/5IJ0Br-wABI?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF")
            };

            videoList.Add(video);

            video = new Video()
            {
                VideoTitle = "Extra English Episode 3 Hector has a date (part2)",
                Url = getLinkVideoFromUrl("https://youtu.be/_KM6TiASSnk?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF"),
                DateAdded = DateTime.Now.ToString("dd-MM-yyyy"),
                ImgUrl = getLinkImgFromUrl("https://youtu.be/_KM6TiASSnk?list=PLdYSWqTrWP2jyqWIdjsATbrb11uN_BMrF")
            };

            videoList.Add(video);
            return videoList;
        }

        public static string getVideoIdFromUrl(string url)
        {
            List<int> index = new List<int>();
            foreach (Match match in Regex.Matches(url, "/"))
                index.Add(match.Index); // Add all index of '/' to array
            url.Remove(0, index[2]); // remove from http:// to YoutubeVideoId #1
            return String.Format("{0}", url.Replace("//", string.Empty));
        }
        private static string getLinkVideoFromUrl(string url)
        {
            if (!url.Contains("watch?v=")){
                List<int> index = new List<int>();
                foreach (Match match in Regex.Matches(url, "/"))
                    index.Add(match.Index); // Add all index of '/' to array
                url.Remove(0, index[2]); // remove from http:// to YoutubeVideoId #1
                url.Remove(url.IndexOf("?")); // Remove from YoutubeVideoId to Last #2
                url = url.Replace("//", String.Empty); // Clear YoutubeID (becuz after 1 & 2 we have : //YoutubeVideoID
                return String.Format("http://youtu.be/{0}", url);
            }
            else if(url.Contains("watch?v=") && url.Contains("list="))
            {
                List<int> index = new List<int>();
                foreach (Match match in Regex.Matches(url, "/"))
                    index.Add(match.Index); // Add all index of '/' to array
                url.Remove(0, index[2]);
                url = url.Replace("watch?v=", string.Empty); // Remove watch?v=
                url = url.Remove(url.IndexOf('&')); // Remove from & to last
                url = url.Replace("//", string.Empty);
                return String.Format("http://youtu.be/{0}", url);
            }
            else
            {
                List<int> index = new List<int>();
                foreach (Match match in Regex.Matches(url, "/"))
                    index.Add(match.Index); // Add all index of '/' to array
                url = url.Remove(0, index[2]);
                url = url.Replace("watch?v=", string.Empty);
                url = url.Replace("//", string.Empty);
                return String.Format("http://youtu.be/{0}", url);
            }
        }

        private static string getLinkImgFromUrl(string url)
        {
            if (!url.Contains("watch?v="))
            {
                List<int> index = new List<int>();
                foreach (Match match in Regex.Matches(url, "/"))
                    index.Add(match.Index);
                url = url.Remove(0, index[2]);
                url = url.Remove(url.IndexOf("?"));
                url = url.Replace("//", string.Empty);
                return String.Format("http://i.ytimg.com/vi{0}/default.jpg", url);
            }
            else if(url.Contains("watch?v=") && url.Contains("list="))
            {
                List<int> index = new List<int>();
                foreach (Match match in Regex.Matches(url, "/"))
                    index.Add(match.Index);
                url = url.Remove(0, index[2]);
                url = url.Replace("watch?v=", string.Empty);
                url = url.Remove(url.IndexOf("&"));
                url = url.Replace("//", string.Empty);
                return String.Format("http://i.ytimg.com/vi{0}/default.jpg", url);
            }
            else
            {
                List<int> index = new List<int>();
                foreach (Match match in Regex.Matches(url, "/"))
                    index.Add(match.Index);
                url = url.Remove(0, index[2]);
                url = url.Replace("watch?v=", string.Empty);
                url = url.Replace("//", string.Empty);
                return String.Format("http://i.ytimg.com/vi{0}/default.jpg", url);
            }
        }
    }

    public class Video
    {
        public string VideoTitle { get; set; }
        public string DateAdded { get; set; }
        public string ImgUrl { get; set; }
        public string Url { get; set; }
    }
}
