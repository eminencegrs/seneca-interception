using Microsoft.Extensions.Logging;

namespace Interception.Core.Settings
{
    public class LoggingSettings
    {
        public bool CanTrackArguments { get; set; } = false;

        public bool IsEnabled { get; set; } = false;

        public LogLevel LogLevel { get; set; } = LogLevel.None;
    }
}