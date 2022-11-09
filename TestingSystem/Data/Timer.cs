using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace TestingSystem.Data
{
    internal class Timer : INotifyPropertyChanged
    {
        private DispatcherTimer timer;

        private int minutes;
        private int seconds;

        public string StringTime
        {
            get
            {
                string stringTime = "";

                if (minutes < 10)
                    stringTime += "0" + minutes.ToString();
                else
                    stringTime += minutes.ToString();

                stringTime += ":";

                if (seconds < 10)
                    stringTime += "0" + seconds.ToString();
                else
                    stringTime += seconds.ToString();

                return stringTime;
            }
            set { }
        }


        public delegate void TimerOverHandler();
        public event TimerOverHandler EndActions;


        private Timer()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
        }
        public Timer(int minutes, int seconds) : this()
        {
            this.minutes = minutes;
            this.seconds = seconds;
        }


        public void StartTimer()
        {
            timer.Start();
        }
        public void StopTimer()
        {
            timer.Stop();
        }

        private void Tick(object sender, EventArgs e)
        {
            if (seconds > 0)
                --seconds;
            else
            {
                if (minutes > 0)
                {
                    --minutes;
                    seconds = 59;
                }
            }

            if (minutes == 0 && seconds == 0)
            {
                timer.Stop();
                EndActions?.Invoke();
            }

            OnPropertyChanged(nameof(StringTime));
        }


        #region PROPERTY_CHANGED METHOD

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
