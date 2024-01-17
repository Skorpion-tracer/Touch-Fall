using TouchFall.Controller.Interfaces;
using TouchFall.Helper.Enums;
using TouchFall.Model;
using TouchFall.Singletons;
using TouchFall.View;
using UnityEngine;

namespace TouchFall.Controller
{
    public sealed class BoundsController : IUpdater
    {
        #region Fields
        private enum MoveBound
        {
            Up,
            Down
        }

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

        private ModifyBounds _currentMod = ModifyBounds.Stay;
        private MoveBound _moveBoundLeft = MoveBound.Down;
        private MoveBound _moveBoundRight = MoveBound.Down;

        private float _startY;
        private float _endY;
        private float _timeRightBoundMove;
        private float _maxTimeRightBoundMove = 2.5f;
        private float _moveLeft;
        private float _moveRight;
        private float _newPosYBottomBound;
        private float _newPosYView;
        private float _newScale;
        private float _newPos;
        private float _posView;
        private bool _startMoveRightBound;
        private bool _isDeacreaseDistanceBounds;
        private bool _isStayBounds;
        private bool _isStartPosition;
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

            ModifyBound.Instance.Modify += OnModifyBounds;
        }

        ~BoundsController()
        {
            ModifyBound.Instance.Modify -= OnModifyBounds;
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            if (_isStartPosition)
                MoveToStartPoint();
            if (_isStayBounds)
                DeacreaseDistanceBound(ref _isStayBounds);

            if (_currentMod == ModifyBounds.Moving)
                Moving();

            if (_isDeacreaseDistanceBounds)
                DeacreaseDistanceBound(ref _isDeacreaseDistanceBounds);
        }

        public void ActivateBounds(bool isActivate)
        {
            _leftBound.gameObject.SetActive(isActivate);
            _rightBound.gameObject.SetActive(isActivate);
        }

        public void ResetBoundsPosition()
        {
            _model.ResetDistanceBound();
            _isStayBounds = true;
            _isStartPosition = true;
            _currentMod = ModifyBounds.Stay;
            StartValuesBounds();
        }
        #endregion

        #region Private Methods
        private void InitBounds(Transform topBound)
        {
            topBound.position = new Vector2(0, _screenBounds.y + _model.TopBoundPositionOffset);
            topBound.localScale = new Vector2(_screenBounds.x * 2, topBound.localScale.y);

            StartValuesBounds();
        }

        private void Moving()
        {
            if (_moveBoundLeft == MoveBound.Down)
            {
                _moveLeft = Mathf.MoveTowards(_moveLeft, _endY, _model.SpeedMove * Time.deltaTime);
                if (_moveLeft >= _endY)
                {
                    _moveBoundLeft = MoveBound.Up;
                }
            }
            else
            {
                _moveLeft = Mathf.MoveTowards(_moveLeft, 0f, _model.SpeedMove * Time.deltaTime);
                if (_moveLeft <= 0f)
                {
                    _moveBoundLeft = MoveBound.Down;
                }
            }
            _leftBound.transform.position = new Vector2(_leftBound.transform.position.x, _startY - _moveLeft);

            if (_startMoveRightBound)
            {
                if (_moveBoundRight == MoveBound.Down)
                {
                    _moveRight = Mathf.MoveTowards(_moveRight, _endY, _model.SpeedMove * Time.deltaTime);
                    if (_moveRight >= _endY)
                    {
                        _moveBoundRight = MoveBound.Up;
                    }
                }
                else
                {
                    _moveRight = Mathf.MoveTowards(_moveRight, 0f, _model.SpeedMove * Time.deltaTime);
                    if (_moveRight <= 0f)
                    {
                        _moveBoundRight = MoveBound.Down;
                    }
                }
                _rightBound.transform.position = new Vector2(_rightBound.transform.position.x, _startY - _moveRight);

                return;
            }

            _timeRightBoundMove += Time.deltaTime;

            if (_timeRightBoundMove >= _maxTimeRightBoundMove)
            {
                _startMoveRightBound = true;
                _timeRightBoundMove = 0f;
            }
        }

        private void MoveToStartPoint()
        {
            _leftBound.transform.position = Vector2.Lerp(_leftBound.transform.position, new Vector2(_leftBound.transform.position.x, _startY), _model.SpeedMove * Time.deltaTime);
            _rightBound.transform.position = Vector2.Lerp(_rightBound.transform.position, new Vector2(_rightBound.transform.position.x, _startY), _model.SpeedMove * Time.deltaTime);

            if (_leftBound.transform.position.y >= _startY && _rightBound.transform.position.y >= _startY)
                _isStartPosition = false;
        }

        private void OnModifyBounds(ModifyBounds modify)
        {
            switch (modify)
            {
                case ModifyBounds.Moving:
                    if (_currentMod == ModifyBounds.Moving) return;
                    _currentMod = ModifyBounds.Moving;
                    _moveLeft = 0f;
                    _moveRight = 0f;
                    _timeRightBoundMove = 0f;
                    _startMoveRightBound = false;
                    return;
                case ModifyBounds.IncreaseDistance:
                    _isDeacreaseDistanceBounds = true;
                    _model.DecreaseDistanceBound();
                    CalculateNewPosBottomBound();
                    return;
                case ModifyBounds.Stay:
                    _isStayBounds = true;
                    _isStartPosition = true;
                    _currentMod = ModifyBounds.Stay;
                    _model.ResetDistanceBound();
                    CalculateNewPosBottomBound();
                    return;
            }
        }

        private void DeacreaseDistanceBound(ref bool isChangeDistance)
        {
            _newScale = Mathf.MoveTowards(_leftView.transform.localScale.y, _model.DistnaceBetweenBounds, _model.SpeedMove * Time.deltaTime);
            _newPos = Mathf.MoveTowards(_leftBottom.localPosition.y, _newPosYBottomBound, _model.SpeedMove * Time.deltaTime);
            _posView = Mathf.MoveTowards(_leftView.transform.localPosition.y, _newPosYView, _model.SpeedMove * Time.deltaTime);

            _leftView.transform.localScale = new Vector2(_leftView.transform.localScale.x, _newScale);
            _leftView.transform.localPosition = new Vector2(_leftView.transform.localPosition.x, _posView);
            _rightView.transform.localScale = new Vector2(_rightView.transform.localScale.x, _newScale);
            _rightView.transform.localPosition = new Vector2(_rightView.transform.localPosition.x, _posView);

            _leftBottom.localPosition = new Vector2(_leftBottom.localPosition.x, _newPos);
            _rightBottom.localPosition = new Vector2(_rightBottom.localPosition.x, _newPos);

            if (_newScale <= _model.DistnaceBetweenBounds && _newPos <= _newPosYBottomBound && _posView <= _newPosYView)
            {
                isChangeDistance = false;
            }
        }

        private void CalculateNewPosBottomBound()
        {
            _newPosYBottomBound = (_leftTop.localPosition.y - _leftTop.localScale.y) - _model.DistnaceBetweenBounds;
            _newPosYView = _leftTop.localPosition.y - (_leftTop.localScale.y * 0.5f) - (_model.DistnaceBetweenBounds * 0.5f);
        }

        private void StartValuesBounds()
        {
            _startY = _screenBounds.y + _model.BoundsVerticalOffset;
            _endY = _startY - _model.DistnaceBetweenBounds;

            _leftBound.transform.position = new Vector2(-_screenBounds.x - (_leftBound.transform.localScale.x * 0.5f), _startY);
            _leftTop.localScale = new Vector2(_leftTop.localScale.x, _screenBounds.y * 2);
            _leftBottom.localScale = new Vector2(_leftBottom.localScale.x, _screenBounds.y * 2);
            _leftBottom.localPosition = new Vector2(_leftBottom.localPosition.x, (_leftTop.localPosition.y - _leftTop.localScale.y) - _model.DistnaceBetweenBounds);
            _leftView.transform.localScale = new Vector2(_leftView.transform.localScale.x, _model.DistnaceBetweenBounds);
            _leftView.transform.localPosition = new Vector2(_leftView.transform.localPosition.x, _leftTop.localPosition.y - (_leftTop.localScale.y * 0.5f) - (_leftView.transform.localScale.y * 0.5f));

            _rightBound.transform.position = new Vector2(_screenBounds.x + (_rightBound.transform.localScale.x * 0.5f), _startY);
            _rightTop.localScale = new Vector2(_rightTop.localScale.x, _screenBounds.y * 2);
            _rightBottom.localScale = new Vector2(_rightBottom.localScale.x, _screenBounds.y * 2);
            _rightBottom.position = new Vector2(_rightBottom.position.x, (_rightTop.position.y - _rightTop.localScale.y) - _model.DistnaceBetweenBounds);
            _rightView.transform.localScale = new Vector2(_rightView.transform.localScale.x, _model.DistnaceBetweenBounds);
            _rightView.transform.position = new Vector2(_rightView.transform.position.x, _rightTop.position.y - (_rightTop.localScale.y * 0.5f) - (_rightView.transform.localScale.y * 0.5f));
        }
        #endregion
    }
}