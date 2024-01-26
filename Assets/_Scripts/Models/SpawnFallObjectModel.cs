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

        private int _maxCountIncrementLevel = 5;
        private int _countIncrementLevel;

        private int _probabilityEmpty;
        private int _probabilityHero;
        private int _probabilityBound;
        private int _probabilityEnemy;
        private int _probabilitySave;

        private int _maxRangeDropEmpty;
        private int _maxRangeDropHero;
        private int _maxRangeDropBound;
        private int _maxRangeDropEnemy;
        private int _maxRangeDropSave;
        #endregion

        #region Constructor
        public SpawnFallObjectModel()
        {
            GameLevel.Instance.ChangeLevel += OnChangeLevel;
            GameLevel.Instance.CreateGameSession += OnCreateGameSession;
            SetProbability();
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

        public int ProbabilityEmpty => _probabilityEmpty;
        public int ProbabilityHero => _probabilityHero;
        public int ProbabilityBound => _probabilityBound;
        public int ProbabilityEnemy => _probabilityEnemy;
        public int ProbabilitySave => _probabilitySave;

        public int MaxRangeDropEmpty => _maxRangeDropEmpty;
        public int MaxRangeDropHero => _maxRangeDropHero;
        public int MaxRangeDropBound => _maxRangeDropBound;
        public int MaxRangeDropEnemy => _maxRangeDropEnemy;
        public int MaxRangeDropSave => _maxRangeDropSave;
        #endregion

        #region Private Methods
        private void OnChangeLevel()
        {
            TimeSpawn -= _decrementTime;
            if (_countIncrementLevel >= _maxCountIncrementLevel) return;
            _countIncrementLevel++;
            ChangeProbability();
        }

        private void OnCreateGameSession()
        {
            TimeSpawn = _maxTimeSpawn;
            _countIncrementLevel = 0;
            SetProbability();
        }

        private void SetProbability()
        {
            _probabilityEmpty = 1;
            _probabilityHero = 4;
            _probabilityBound = 2;
            _probabilityEnemy = 3;
            _probabilitySave = 4;

            _maxRangeDropEmpty = 6;
            _maxRangeDropHero = 6;
            _maxRangeDropBound = 6;
            _maxRangeDropEnemy = 6;
            _maxRangeDropSave = 6;
        }

        private void ChangeProbability()
        {
            _probabilityEmpty++;
            _probabilityHero = _countIncrementLevel <= 2 ? (_probabilityHero - 1) : _probabilityHero;
            _probabilityBound = _countIncrementLevel == 3 ? (_probabilityBound - 1) : _probabilityBound;
            _probabilityEnemy = _countIncrementLevel >= 3 ? (_probabilityEnemy - 1) : _probabilityEnemy;
            _probabilitySave = _countIncrementLevel >= 3 ? (_probabilitySave - 1) : _probabilitySave;

            if (_countIncrementLevel >= 3)
            {
                _maxRangeDropEnemy++;
                _probabilitySave++;
            }
        }
        #endregion
    }
}
