using TouchFall.Controller.Interfaces;
using TouchFall.Helper;
using TouchFall.Helper.Enums;
using TouchFall.Input;
using TouchFall.Model;
using TouchFall.View;
using UnityEngine;
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

        private Vector2 _startPosition;
        private Vector2 _endPosition;

        private float _downTime = 0f;
        private float a;
        #endregion

        #region Constructors
        /// <summary>
        /// »нициализаци€ контроллера
        /// </summary>
        /// <param name="mainHeroView">—сылка на view геро€</param>
        /// <param name="mainHeroModel">—сылка на модель геро€</param>
        /// <param name="startPosition">—сылка на стартовую позицию геро€</param>
        public MainHeroMoveController(MainHeroView mainHeroView, MainHeroModel mainHeroModel, PlayerControl playerControl, Vector2 startPosition)
        {
            _view = mainHeroView;
            _model = mainHeroModel;
            _startPosition = startPosition;

            //_heroTransform = _view.transform;

            _camera = Camera.main;

            _playerControl = playerControl;

            _playerControl.Touch.PrimaryContact.started += ctx => OnStartTouch(ctx);
            _playerControl.Touch.PrimaryContact.canceled += ctx => EndTocuhPrimary(ctx);
        }

        ~MainHeroMoveController()
        {
            _playerControl.Touch.PrimaryContact.started -= ctx => OnStartTouch(ctx);
            _playerControl.Touch.PrimaryContact.canceled -= ctx => EndTocuhPrimary(ctx);
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            switch (_model.StateMainHero)
            {
                case StateMoveMainHero.None:
                case StateMoveMainHero.End: return;

                case StateMoveMainHero.Touch:
                    _view.Transform.position = new Vector3(Mathf.Clamp(_view.Transform.position.x, -_model.ClampMove, _model.ClampMove), 
                        Mathf.Clamp(_view.Transform.position.y, -(_model.ClampMove * 2), (_model.ClampMove * 2)), _view.Transform.position.z);
                    Debug.Log(_view.Transform.position);
                    break;
                case StateMoveMainHero.EndTouch:
                    _view.Transform.position = new Vector3(Mathf.Clamp(_view.Transform.position.x, -_model.ClampMove, _model.ClampMove), 
                        Mathf.Clamp(_view.Transform.position.y, -(_model.ClampMove * 2), (_model.ClampMove * 2)), _view.transform.position.z);

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
            _model.StateMainHero = StateMoveMainHero.Touch;
        }

        private void EndTocuhPrimary(InputAction.CallbackContext ctx)
        {
            _model.StateMainHero = StateMoveMainHero.EndTouch;
            _endPosition = GetPositionTouch();
        }

        private Vector3 GetPositionTouch()
        {
            return Utils.ScreenToWorld(_camera, _playerControl.Touch.PrimaryPosition.ReadValue<Vector2>());
        }
        #endregion
    }
}
