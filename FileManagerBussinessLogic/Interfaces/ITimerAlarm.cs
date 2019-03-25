using System;


namespace FileManagerBussinessLogic.Interfaces
{
    public interface ITimerAlarm
    {
        void StartTimerEvent();
        Action Callback { get; set; }
    }
}
