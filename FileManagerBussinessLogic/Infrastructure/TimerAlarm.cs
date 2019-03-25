using System;
using System.Timers;
using FileManagerBussinessLogic.Interfaces;

namespace FileManagerBussinessLogic.Infrastructure
{
     public class TimerAlarm:ITimerAlarm
    {
        public Action Callback { get; set; }
        private static Timer timer;


        public void StartTimerEvent()
        {
            timer = new Timer();
            timer.Interval = 2000;

            timer.Elapsed += (source, e) => Callback();
            timer.AutoReset = true;
            timer.Enabled = true;

        }
    }
}
