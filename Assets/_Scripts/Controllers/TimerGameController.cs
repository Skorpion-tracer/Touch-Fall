using TouchFall.Controller.Interfaces;
using TouchFall.Model;
using TouchFall.Singletons;
using UnityEngine;

namespace TouchFall.Controller
{
    public sealed class TimerGameController : IUpdater
    {
        #region Fields
        private readonly TimerGameModel _model;

        private float _time = 0f;
        #endregion

        #region Constructor
        public TimerGameController(TimerGameModel model)
        {
            _model = model;
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            _time += Time.deltaTime;
            if (_time >= _model.TimeLimit)
            {
                _time = 0f;
                if (_model.IsTimeNonMax())
                {
                    GameLevel.Instance.IncrementLevel();
                    _model.IncrementTime();
                }
            }
        }

        public void Reset()
        {
            _time = 0f;
        }
        #endregion
    }
}
