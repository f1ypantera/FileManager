using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace FileManagerAPI.Infrastructure
{
    
    public class TimerAlarm:ITimeAlarm
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
