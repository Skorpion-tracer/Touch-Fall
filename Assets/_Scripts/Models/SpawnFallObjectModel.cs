using System;
using TouchFall.Singletons;
using UnityEngine;

namespace TouchFall.Model
{
    [Serializable]
    public sealed class SpawnFallObjectModel
    {
        #region Fields
        [SerializeField, Range(0.5f, 3)] private float _timeSpawn;
        [SerializeField, Range(1, 10)] private float _offsetVerticalPositionSpawn;

        private float _minTimeSpawn = 0.5f;
        private float _maxTimeSpawn = 3f;
        private float _decrementTime = 0.65f;
        #endregion

        #region Constructor
        public SpawnFallObjectModel()
        {
            GameLevel.Instance.ChangeLevel += OnChangeLevel;
            GameLevel.Instance.CreateGameSession += OnCreateGameSession;
        }

        ~SpawnFallObjectModel()
        {
            GameLevel.Instance.ChangeLevel -= OnChangeLevel;
            GameLevel.Instance.CreateGameSession -= OnCreateGameSession;
        }
        #endregion

        #region Properties
        public float TimeSpawn
        {
            get => _timeSpawn;
            set
            {
                if (value < _minTimeSpawn)
                {
                    _timeSpawn = _minTimeSpawn;
                }
                else if (value > _maxTimeSpawn)
                {
                    _timeSpawn = _maxTimeSpawn;
                }
                else
                {
                    _timeSpawn = value;
                }
            }
        }

        public float OffsetVerticalPositionSpawn => _offsetVerticalPositionSpawn;
        #endregion

        #region Private Methods
        private void OnChangeLevel()
        {
            TimeSpawn -= _decrementTime;
        }

        private void OnCreateGameSession()
        {
            TimeSpawn = _maxTimeSpawn;
        }
        #endregion
    }
}
