using System;
using System.IO;
using System.Threading;
using TouchFall.Helper;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace TouchFall.Singletons
{
    public sealed class GameData
    {
        #region Fields
        private static readonly Lazy<GameData> _instance =
            new(() => new GameData(), LazyThreadSafetyMode.ExecutionAndPublication);

        private SaveData _saveData;
        private readonly string _saveFolder = Path.Combine(Application.dataPath, "Saves");
        private readonly string _name;
        #endregion

        #region Constructor
        private GameData()
        {
            _saveData = new();
            if (!Directory.Exists(_saveFolder))
            {
                Directory.CreateDirectory(_saveFolder);
            }
            _name = Path.Combine(_saveFolder, "save.txt");
        }
        #endregion

        #region Properties
        public static GameData Instance => _instance.Value;
        public SaveData SaveData => _saveData;
        #endregion

        #region Public Methods
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

        public void Load()
        {
            if (File.Exists(_name))
            {
                string jsonString = File.ReadAllText(_name);

                _saveData = JsonUtility.FromJson<SaveData>(jsonString);
            }
        }
        #endregion

        #region Private Methods
        private void Save()
        {
            string jsonString = JsonUtility.ToJson(_saveData);

            File.WriteAllText(_name, jsonString);
        }
        #endregion
    }
}
