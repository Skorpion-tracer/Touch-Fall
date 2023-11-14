using TouchFall.Controller.Interfaces;
using TouchFall.Helper.Enums;
using TouchFall.Input;
using TouchFall.View;
using UnityEngine;

namespace TouchFall.Controller
{
    public sealed class MainHeroController : IUpdater, IFixedUpdater
    {
        #region Fields
        private MainHeroView _view;
        private MainHeroModel _model;

        private InputManager _inputManager;
        private Transform _heroTransform;

        private Vector2 _startPosition;
        private Vector2 _endPosition;
        #endregion

        #region Constructors
        /// <summary>
        /// »нициализаци€ контроллера
        /// </summary>
        /// <param name="mainHeroView">—сылка на view геро€</param>
        /// <param name="mainHeroModel">—сылка на модель геро€</param>
        /// <param name="startPosition">—сылка на стартовую позицию геро€</param>
        public MainHeroController(MainHeroView mainHeroView, MainHeroModel mainHeroModel, Vector2 startPosition)
        {
            _view = mainHeroView;
            _model = mainHeroModel;
            _startPosition = startPosition;

            _heroTransform = _view.transform;

            _inputManager = InputManager.Instance;

            _view.EnableView += OnEnableView;
            _view.DisableView += OnDisableView;
        }

        ~MainHeroController()
        {
            _view.EnableView -= OnEnableView;
            _view.DisableView -= OnDisableView;
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            switch (_model.InputState)
            {
                case InputState.Touch:
                    _heroTransform.position = new Vector3(Mathf.Clamp(_heroTransform.position.x, -2f, 2f), Mathf.Clamp(_heroTransform.position.y, -4f, 4f), _heroTransform.position.z);
                    break;
                case InputState.EndTouch:
                    _heroTransform.position = new Vector3(Mathf.Clamp(_heroTransform.position.x, -2f, 2f), Mathf.Clamp(_heroTransform.position.y, -4f, 4f), _heroTransform.position.z);
                    if (_heroTransform.position == (Vector3)_endPosition)
                    {
                        _model.InputState = InputState.None;
                    }
                    break;
            }
        }

        public void FixedUpdate()
        {
            switch (_model.InputState)
            {
                case InputState.Touch:
                    _view.Body.MovePosition(Vector3.Lerp(_heroTransform.position, _inputManager.PrimaryPosition(), _model.Speed * Time.fixedDeltaTime));
                    break;
                case InputState.EndTouch:
                    _view.Body.MovePosition(Vector3.Lerp(_heroTransform.position, _endPosition, _model.Speed * Time.fixedDeltaTime));
                    break;
            }
        }
        #endregion

        #region Private Mathods
        private void OnEnableView()
        {
            _inputManager.StartTouch += OnStartSwipe;
            _inputManager.EndTouch += OnEndSwipe;
        }

        private void OnDisableView()
        {
            _inputManager.StartTouch -= OnStartSwipe;
            _inputManager.EndTouch -= OnEndSwipe;
        }

        private void OnStartSwipe(Vector2 position, float time)
        {
            _model.InputState = InputState.Touch;
        }

        private void OnEndSwipe(Vector2 position, float time)
        {
            _model.InputState = InputState.EndTouch;
            _endPosition = position;
        }
        #endregion
    }
}
