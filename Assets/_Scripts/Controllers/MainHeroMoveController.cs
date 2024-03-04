using System;
using TouchFall.Controller.Interfaces;
using TouchFall.Helper;
using TouchFall.Helper.Enums;
using TouchFall.Input;
using TouchFall.Model;
using TouchFall.Singletons;
using TouchFall.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TouchFall.Controller
{
    public sealed class MainHeroMoveController : IUpdater, IFixedUpdater
    {
        #region Fields
        private MainHeroView _view;
        private MainHeroModel _model;

        private PlayerControl _playerControl;
        //private Transform _heroTransform;
        private Camera _camera;
        private Func<Vector3> _getTouch;

        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private Vector2 _screenBounds;

        private float _downTime = 0f;
        private float _coeffSizeHero = 0.5f;
        #endregion

        #region Constructors
        /// <summary>
        /// »нициализаци€ контроллера
        /// </summary>
        /// <param name="mainHeroView">—сылка на view геро€</param>
        /// <param name="mainHeroModel">—сылка на модель геро€</param>
        /// <param name="startPosition">—сылка на стартовую позицию геро€</param>
        public MainHeroMoveController(MainHeroView mainHeroView, MainHeroModel mainHeroModel, PlayerControl playerControl, Vector2 startPosition, Vector2 screenBounds)
        {
            _view = mainHeroView;
            _model = mainHeroModel;
            _startPosition = startPosition;

            //_heroTransform = _view.transform;

            _camera = Camera.main;
            _screenBounds = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));

            _playerControl = playerControl;

            if (Application.isMobilePlatform)
            {
                _playerControl.Touch.PrimaryContact.started += ctx => OnStartTouch(ctx);
                _playerControl.Touch.PrimaryContact.canceled += ctx => EndTocuhPrimary(ctx);

                _getTouch = TouchVector3;
            }
            else
            {
                _playerControl.Click.PrimaryContact.started += ctx => OnStartTouch(ctx);
                _playerControl.Click.PrimaryContact.canceled += ctx => EndTocuhPrimary(ctx);

                _getTouch = ClickVector3;
            }
        }

        ~MainHeroMoveController()
        {
            if (Application.isMobilePlatform)
            {
                _playerControl.Touch.PrimaryContact.started -= ctx => OnStartTouch(ctx);
                _playerControl.Touch.PrimaryContact.canceled -= ctx => EndTocuhPrimary(ctx);
            }
            else
            {
                _playerControl.Click.PrimaryContact.started -= ctx => OnStartTouch(ctx);
                _playerControl.Click.PrimaryContact.canceled -= ctx => EndTocuhPrimary(ctx);
            }
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            switch (_model.StateMainHero)
            {
                case StateMoveMainHero.EndTouch:
                    _downTime += Time.deltaTime;
                    if (_downTime >= _model.DownTime)
                    {
                        _model.StateMainHero = StateMoveMainHero.End;
                        _downTime = 0f;
                    }
                    break;
            }
        }

        public void FixedUpdate()
        {
            switch (_model.StateMainHero)
            {
                case StateMoveMainHero.None:
                    _view.Body.MovePosition(Vector3.Lerp(_view.Transform.position, _startPosition, _model.Speed * Time.fixedDeltaTime));
                    return;
                case StateMoveMainHero.Touch:
                    _view.Body.MovePosition(Vector3.Lerp(_view.Transform.position, GetPositionTouch(), _model.Speed * Time.fixedDeltaTime));
                    break;
                case StateMoveMainHero.EndTouch:
                    _view.Body.MovePosition(Vector3.Lerp(_view.Transform.position, _endPosition, _model.Speed * Time.fixedDeltaTime));
                    break;
                case StateMoveMainHero.End:
                    if (_view.Transform.position == (Vector3)_startPosition)
                    {
                        _model.StateMainHero = StateMoveMainHero.None;
                        return;
                    }
                    _view.Body.MovePosition(Vector3.Lerp(_view.Transform.position, _startPosition, _model.Speed * Time.fixedDeltaTime));
                    break;
            }
        }
        #endregion

        #region Private Mathods
        private void OnStartTouch(InputAction.CallbackContext ctx)
        {
            if (GameLoop.Instance.GameState == GameState.GamePlay)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                _model.StateMainHero = StateMoveMainHero.Touch;
            }
        }

        private void EndTocuhPrimary(InputAction.CallbackContext ctx)
        {
            if (GameLoop.Instance.GameState == GameState.GamePlay)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                _model.StateMainHero = StateMoveMainHero.EndTouch;
                _endPosition = GetPositionTouch();
            }
        }

        private Vector3 GetPositionTouch()
        {
            Vector3 pos = _getTouch();

            pos.x = Mathf.Clamp(pos.x, _screenBounds.x * -1 + _coeffSizeHero, _screenBounds.x - _coeffSizeHero);
            pos.y = Mathf.Clamp(pos.y, _screenBounds.y * -1 + _coeffSizeHero, _screenBounds.y - _coeffSizeHero);

            return pos;
        }

        private Vector3 TouchVector3()
        {
            return Utils.ScreenToWorld(_camera, _playerControl.Touch.PrimaryPosition.ReadValue<Vector2>());
        }

        private Vector3 ClickVector3()
        {
            return Utils.ScreenToWorld(_camera, _playerControl.Click.PrimaryPosition.ReadValue<Vector2>());
        }
        #endregion
    }
}
