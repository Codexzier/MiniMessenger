using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MiniMessenger.Components.UserSettings
{
    public class UserSettingsLoader
    {
        private static UserSettingsLoader _userSettings;
        private readonly string _settingFile = $"{Environment.CurrentDirectory}\\settings.json";

        private UserSettingsLoader()
        {
        }

        public static UserSettingsLoader GetInstance()
        {
            if(_userSettings == null)
            {
                _userSettings = new UserSettingsLoader();
            }

            return _userSettings;
        }

        internal SettingsFile Settings => this.LoadSettings();

        private SettingsFile LoadSettings()
        {
            if (!File.Exists(this._settingFile))
            {
                var firstSetting = new SettingsFile
                {
                    Username = "Benutzername",
                    ServerAddress = "http://localhost:5000/",
                    ServerAddressItems = new List<string>() { "http://localhost:5000/" },
                    Interval = 1
                };

                this.SaveToJsonFile(firstSetting);
            }

            var fileContent = File.ReadAllText(this._settingFile);

            var setting = JsonConvert.DeserializeObject<SettingsFile>(fileContent);

            SetupDefault(setting);

            return setting;
        }

        private static void SetupDefault(SettingsFile setting)
        {
            if (setting.ServerAddressItems == null)
            {
                setting.ServerAddressItems = new List<string>() { setting.ServerAddress };
            }

            if (setting.Interval <= 0)
            {
                setting.Interval = 1;
            }
        }

        private void SaveToJsonFile(SettingsFile firstSetting)
        {
            var toSave = JsonConvert.SerializeObject(firstSetting);
            File.WriteAllText(this._settingFile, toSave);
        }

        internal void Save(SettingsFile settingsFile) => this.SaveToJsonFile(settingsFile);
    }
}
