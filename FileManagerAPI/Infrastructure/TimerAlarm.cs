using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace FileManagerAPI.Infrastructure
{
    public class TimerAlarm
    {
        private static Timer timer;
        private FileManager fileManager;
        public TimerAlarm(FileManager fileManager)
        {
            this.fileManager = fileManager;
        }

   
        public void StartTimerEvent()
        {
            timer = new Timer();
            timer.Interval = 2000;

            timer.Elapsed += (source, e) => fileManager.CheckFile(source, e);
            timer.AutoReset = true;
            timer.Enabled = true;

        }
    }
}
