using System;
using System.Timers;

namespace TrialOfTheSphinx.Core
{
    // Simple countdown timer that raises events every second
    public class TimerSystem
    {
        private Timer timer;
        private int seconds;
        private int totalSeconds;

        // Event fired every second with remaining time
        public event Action<TimeSpan> TimeUpdated;

        // Event fired when timer reaches zero
        public event Action TimeExpired;

        public TimerSystem(int seconds)
        {
            this.seconds = seconds;
            totalSeconds = seconds;

            // Timer ticks every 1000 ms (1 second)
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
        }

        // Called each time the timer ticks
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            seconds--;
            TimeUpdated?.Invoke(TimeSpan.FromSeconds(seconds));

            // Stop timer and notify when time is up
            if (seconds <= 0)
            {
                timer.Stop();
                TimeExpired?.Invoke();
            }
        }

        // Start counting down
        public void Start() => timer.Start();

        // Stop counting down
        public void Stop() => timer.Stop();

        // Original total time set
        public int TotalSeconds => totalSeconds;
    }
}
