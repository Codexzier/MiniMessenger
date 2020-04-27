using System.Collections.Generic;

namespace MiniMessenger.Components.UserSettings
{
    internal class SettingsFile
    {
        public string ServerAddress { get; set; }

        public IEnumerable<string> ServerAddressItems { get; set; }

        public string Username { get; set; }

        public int Interval { get; set; }
    }
}