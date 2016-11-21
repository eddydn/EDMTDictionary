using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace EDMTDictionary.Helpers
{
    
    public class Library
    {
        //Here Library for working with Youtube
        public delegate void PlayingEvent();
        public event PlayingEvent Playing;
        private DispatcherTimer timer = new DispatcherTimer();

        public void Init()
        {
            timer.Tick += (object sender, object e) => {
                if (Playing != null)
                    Playing();
                    
                    };
        }

        public void Timer(bool Enable)
        {
            if (Enable) timer.Start();
            else timer.Stop();
        }
    }
}
