using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TouchFall.Helper;
using TouchFall.Helper.Enums;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace TouchFall.Singletons
{
    public sealed class GameData
    {
        #region Fields
        private static readonly Lazy<GameData> _instance =
            new(() => new GameData(), LazyThreadSafetyMode.ExecutionAndPublication);

        private SaveData _saveData;
        private Yandex _yandex;
        private Dictionary<string, Language> _langDict = new()
        {
            { "ru", Language.Russian }, { "en", Language.English }
        };
        #endregion

        #region Constructor
        private GameData()
        {
            _saveData = new();
        }
        #endregion

        #region Properties
        public static GameData Instance => _instance.Value;
        public SaveData SaveData => _saveData;
        #endregion

        #region Public Methods
        public void InitYandex(Yandex yandex)
        {
            _yandex = yandex;
        }

        public void Save(int score, bool isOnOffMusic, Language language)
        {
            _saveData.scores = score;
            _saveData.isOnMusic = isOnOffMusic;
            _saveData.language = language;

            Save();
        }

        public void Save(int score, bool isOnOffMusic)
        {
            _saveData.scores = score;
            _saveData.isOnMusic = isOnOffMusic;

            Save();
        }

        public void Save(bool isOnOffMusic)
        {
            _saveData.isOnMusic = isOnOffMusic;

            Save();
        }

        public void Save(int scores)
        {
            _saveData.scores = scores;

            Save();
        }

        public void Save(Language language)
        {
            _saveData.isChangeLang = true;
            _saveData.language = language;

            Save();
        }

        public void Load()
        {
            _yandex.Load();
        }

        public void Load(string data)
        {
            _saveData = JsonUtility.FromJson<SaveData>(data);
        }

        public void Localize(string lang)
        {
            if (_langDict.ContainsKey(lang))
            {
                _saveData.language = _langDict[lang];

                LocalizationSettings.InitializationOperation.Completed += e =>
                {
                    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)_saveData.language];
                };
            }
            else
            {
                LocalizationSettings.InitializationOperation.Completed += e =>
                {
                    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)Language.Russian];
                };
            }
        }
        #endregion

        #region Private Methods
        private void Save()
        {
            string jsonString = JsonUtility.ToJson(_saveData);
            _yandex.Save(jsonString);
        }
        #endregion
    }
}
