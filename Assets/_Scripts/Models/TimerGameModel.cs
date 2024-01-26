using System;
using UnityEngine;

namespace TouchFall.Model
{
    [Serializable]
    public sealed class TimerGameModel
    {
        #region Fields
        [SerializeField, Range(50, 120)] private float _timeLimit = 50f;

        private float _timeIncrement = 30f;
        private float _timeStart = 50f;
        private float _timeMin = 50f;
        private float _timeMax = 120f;
        #endregion

        #region Properties
        public float TimeLimit
        {
            get => _timeLimit;
            set
            {
                if (value < _timeMin)
                {
                    value = _timeMin;
                }
                else if (value > _timeMax)
                {
                    value = _timeMax;
                }
                _timeLimit = value;
            }
        }
        #endregion

        #region Public Methods
        public void IncrementTime()
        {
            TimeLimit += _timeIncrement;
        }

        public bool IsTimeNonMax()
        {
            return TimeLimit <= _timeMax;
        }

        public void Reset()
        {
            TimeLimit = _timeStart;
        }
        #endregion
    }
}
