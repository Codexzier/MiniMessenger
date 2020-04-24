using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MiniMessenger.Components.UserSettings
{
    public class UserSettingsLoader
    {
        private static UserSettingsLoader _userSettings;
        private string _settingFile = $"{Environment.CurrentDirectory}\\settings.json";

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

        public string GetUrl()
        {
            var setting = this.LoadSettings();

            return setting.ServerAddress;
        }

        private SettingsFile LoadSettings()
        {
            if(!File.Exists(this._settingFile))
            {
                var firstSetting = new SettingsFile
                {
                    Username = "Benutzername",
                    ServerAddress = "http://localhost:5000/",
                    ServerAddressItems = new List<string>() { "http://localhost:5000/" }
                };

                this.SaveToJsonFile(firstSetting);
            }

            var fileContent = File.ReadAllText(this._settingFile);

            var setting = JsonConvert.DeserializeObject<SettingsFile>(fileContent);

            if(setting.ServerAddressItems == null)
            {
                setting.ServerAddressItems = new List<string>() { setting.ServerAddress };
            }

            return setting;
        }

        private void SaveToJsonFile(SettingsFile firstSetting)
        {
            var toSave = JsonConvert.SerializeObject(firstSetting);
            File.WriteAllText(this._settingFile, toSave);
        }

        public string GetUsername()
        {
            return this.LoadSettings().Username;
        }

        internal void Save(SettingsFile settingsFile) => this.SaveToJsonFile(settingsFile);
    }
}
