using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.Infrastructure
{
    public interface ITimeAlarm
    {
        void StartTimerEvent();
        Action Callback { get; set; }
    }
}
