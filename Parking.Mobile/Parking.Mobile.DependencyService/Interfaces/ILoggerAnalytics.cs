using System;
using Parking.Mobile.DependencyService.Enum;

namespace Parking.Mobile.DependencyService.Interfaces
{
    public interface ILoggerAnalytics
    {
        public void LogEvent(LogEventType eventType, string key, string value);
    }
}

