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

        private Transform _leftTop;
        private WindowView _leftView;
        private Transform _leftBottom;

        private Transform _rightTop;
        private WindowView _rightView;
        private Transform _rightBottom;

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

            _leftTop = _leftBound.Bounds.topBound;
            _leftView = _leftBound.Bounds.window;
            _leftBottom = _leftBound.Bounds.bottomBound;

            _rightTop = _rightBound.Bounds.topBound;
            _rightView = _rightBound.Bounds.window;
            _rightBottom = _rightBound.Bounds.bottomBound;

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

            _leftBound.transform.position = new Vector2(-_screenBounds.x - (_leftBound.transform.localScale.x * 0.5f), _screenBounds.y + _model.BoundsVerticalOffset);
            _leftTop.localScale = new Vector2(_leftTop.localScale.x, _screenBounds.y * 2);
            _leftBottom.localScale = new Vector2(_leftBottom.localScale.x, _screenBounds.y * 2);
            _leftBottom.position = new Vector2(_leftBottom.position.x, (_leftTop.position.y - _leftTop.localScale.y) - _model.DistnaceBetweenBounds);
            _leftView.transform.localScale = new Vector2(_leftView.transform.localScale.x, _model.DistnaceBetweenBounds);
            _leftView.transform.position = new Vector2(_leftView.transform.position.x, _leftTop.position.y - (_leftTop.localScale.y * 0.5f) - (_leftView.transform.localScale.y * 0.5f));

            BoxCollider2D collider = _rightView.GetComponent<BoxCollider2D>();
            if (collider != null)
                collider.offset = new Vector2(collider.offset.x * -1, collider.offset.y);
            
            _rightBound.transform.position = new Vector2(_screenBounds.x + (_rightBound.transform.localScale.x * 0.5f), _screenBounds.y + _model.BoundsVerticalOffset);
            _rightTop.localScale = new Vector2(_rightTop.localScale.x, _screenBounds.y * 2);
            _rightBottom.localScale = new Vector2(_rightBottom.localScale.x, _screenBounds.y * 2);
            _rightBottom.position = new Vector2(_rightBottom.position.x, (_rightTop.position.y - _rightTop.localScale.y) - _model.DistnaceBetweenBounds);
            _rightView.transform.localScale = new Vector2(_rightView.transform.localScale.x, _model.DistnaceBetweenBounds);
            _rightView.transform.position = new Vector2(_rightView.transform.position.x, _rightTop.position.y - (_rightTop.localScale.y * 0.5f) - (_rightView.transform.localScale.y * 0.5f));
        }
        #endregion
    }
}