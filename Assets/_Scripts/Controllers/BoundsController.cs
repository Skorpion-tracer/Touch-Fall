using TouchFall.Controller.Interfaces;
using TouchFall.Model;
using TouchFall.View;
using UnityEngine;

namespace TouchFall.Controller
{
    public sealed class BoundsController : IUpdater
    {
        #region Fields
        private BoundView _leftBound;
        private BoundView _rightBound;
        private BoundModel _model;
        private Vector2 _screenBounds;
        #endregion

        #region Constructor
        public BoundsController(Vector2 screenBounds, BoundView leftBound, BoundView rightBound, BoundModel boundModel, Transform topBound)
        {
            _screenBounds = screenBounds;

            _leftBound = leftBound;
            _rightBound = rightBound;

            _model = boundModel;

            InitBounds(topBound);
        }
        #endregion

        #region Public Methods
        public void Update()
        {

        }
        #endregion

        #region Private Methods
        private void InitBounds(Transform topBound)
        {
            topBound.position = new Vector2(0, _screenBounds.y + _model.TopBoundPositionOffset);
            topBound.localScale = new Vector2(_screenBounds.x * 2, topBound.localScale.y);
        }
        #endregion
    }
}