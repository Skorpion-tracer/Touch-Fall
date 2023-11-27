using System;
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
    }
}
