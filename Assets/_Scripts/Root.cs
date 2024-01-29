using System.Collections.Generic;
using System.Linq;
using TouchFall.Controller;
using TouchFall.Controller.Interfaces;
using TouchFall.Helper;
using TouchFall.Helper.Enums;
using TouchFall.Helper.PoolObject;
using TouchFall.Input;
using TouchFall.Model;
using TouchFall.Singletons;
using TouchFall.View;
using TouchFall.View.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TouchFall
{
    public sealed class Root : MonoBehaviour
    {
        #region SerializeFields
        [Header("Hero")]
        [SerializeField] private Transform _startPointHero;
        [SerializeField] private MainHeroView _mainHero;
        [SerializeField] private MainHeroModel _mainHeroModel;

        [Space(5f), Header("Bounds")]
        [SerializeField] private Transform _postionTopBound;
        [SerializeField] private BoundView _leftBoundView;
        [SerializeField] private BoundView _rightBoundView;
        [SerializeField] private BottomTriggerView _bottomTriggerView;
        [SerializeField] private BoundModel _boundModel;

        [Space(5f), Header("Spawner")]
        [SerializeField] private PoolEmptyObject _poolEmptyObject;
        [SerializeField] private PoolModifyObject _poolModifyObject;
        [SerializeField] private PoolModifyBoundObject _poolModifyBoundObject;
        [SerializeField] private PoolEnemyObject _poolEnemyObject;
        [SerializeField] private PoolNeedToSaveObject _poolNeedToSaveObject;
        [SerializeField] private SpawnFallObjectModel _spawnModel;

        [Space(5f), Header("GameLevel")]
        [SerializeField] private TimerGameModel _timerGameModel;

        [Space(5f), Header("UI")]
        [SerializeField] private UIGamePlayDispatcher _uiGame;
        [SerializeField] private UIMainMenuDispatcher _uiMenu;
        [SerializeField] private Volume _volume;
        #endregion

        #region Fields
        private MainHeroMoveController _mainHeroMoveController;
        private MainHeroBehavoiurController _mainHeroBehavoiurController;
        private BoundsController _boundsController;
        private PlayerControl _playerControl;
        private SpawnFallObjectController _spawnController;
        private TimerGameController _timerGameController;
        private List<IUpdater> _updaters = new();
        private List<IFixedUpdater> _fixedUpdaters = new();
        private PoolContainer _poolContainer;
        private DepthOfField _blur;

        private Vector2 _screenBounds;
        private float _blurValue = 100f;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            _playerControl?.Enable();
            GameLevel.Instance.CreateGameSession += OnCreateGameSession;
            GameLoop.Instance.PauseBegin += OnPauseBegin;
            GameLoop.Instance.ResumeCommercial += OnResumeCommercial;
            GameLevel.Instance.ExitMenu += OnExitMenu;
            GameLevel.Instance.GameOver += GameOver;
        }

        private void OnDisable()
        {
            _playerControl?.Disable();
            GameLevel.Instance.CreateGameSession -= OnCreateGameSession;
            GameLoop.Instance.PauseBegin -= OnPauseBegin;
            GameLoop.Instance.ResumeCommercial -= OnResumeCommercial;
            GameLevel.Instance.ExitMenu -= OnExitMenu;
            GameLevel.Instance.GameOver -= GameOver;
        }

        private void Awake()
        {
            InitModels();

            GameData.Instance.Load();

            _uiMenu.ShowBestPoints(GameData.Instance.SaveData.scores > 0);
            _uiMenu.UpdateBestBoint(GameData.Instance.SaveData.scores);

            _playerControl = new();
            _playerControl.Click.Pause.started += OnKeyboardEsc;

            _screenBounds = Utils.ScreenToWorld(Camera.main, new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

            InitPools();

            InitControllers();

            InitUpdaters();

            _blur = (DepthOfField)_volume.profile.components.FirstOrDefault(e => e is DepthOfField);
            _blur.focalLength.value = _blurValue;
        }

        private void OnDestroy()
        {
            _playerControl.Click.Pause.started -= OnKeyboardEsc;
        }

        private void Update()
        {
            if (GameLoop.Instance.GameState == GameState.MainMenu) return;
            if (GameLoop.Instance.GameState == GameState.GamePlay)
            {
                for (int i = 0; i < _updaters.Count; i++)
                {
                    _updaters[i].Update();
                }
            }
            BlurControl();
        }

        private void FixedUpdate()
        {
            if (GameLoop.Instance.GameState == GameState.GamePlay)
            {
                for (int i = 0; i < _fixedUpdaters.Count; i++)
                {
                    _fixedUpdaters[i].FixedUpdate();
                }
            }
        }

        private void InitModels()
        {
            _mainHeroModel ??= new MainHeroModel();
            _boundModel ??= new BoundModel();
            _boundModel.SetStartDistanceBound();
            _spawnModel ??= new SpawnFallObjectModel();
            _timerGameModel ??= new TimerGameModel();
        }

        private void InitPools()
        {
            _poolEmptyObject.InitPool();
            _poolModifyObject.InitPool();
            _poolModifyBoundObject.InitPool();
            _poolEnemyObject.InitPool();
            _poolNeedToSaveObject.InitPool();

            _poolContainer = new(_poolEmptyObject, _poolModifyObject, _poolModifyBoundObject, _poolEnemyObject, _poolNeedToSaveObject);
        }

        private void InitControllers()
        {
            BoundView leftBound = Instantiate(_leftBoundView, Vector2.zero, Quaternion.identity);
            BoundView rightBound = Instantiate(_rightBoundView, Vector2.zero, Quaternion.identity);

            _boundsController = new(_screenBounds, leftBound, rightBound, _boundModel, _postionTopBound);
            _boundsController.ActivateBounds(false);

            _bottomTriggerView.Initialized(_boundModel, _screenBounds);

            _mainHero.InstantiateHeroes(_startPointHero.position);
            _mainHeroMoveController = new(_mainHero, _mainHeroModel, _playerControl, _startPointHero.position);
            _mainHeroBehavoiurController = new(_mainHero, _mainHeroModel);

            _spawnController = new(_spawnModel, _poolContainer, _screenBounds);
            _timerGameController = new(_timerGameModel);
        }

        private void InitUpdaters()
        {
            _updaters.Add(_mainHeroMoveController);
            _updaters.Add(_boundsController);
            _updaters.Add(_spawnController);
            _updaters.Add(_timerGameController);

            _fixedUpdaters.Add(_mainHeroMoveController);
            _fixedUpdaters.Add(_mainHeroBehavoiurController);
        }

        private void OnCreateGameSession()
        {
            _poolContainer.PauseAllObjects(false);
            _poolContainer.ResetObjects();
            _mainHero.ResetPlayer(_startPointHero.position);
            _boundsController.ActivateBounds(true);
            _boundsController.ResetBoundsPosition();
            _timerGameModel.Reset();
            _timerGameController.Reset();
            GameLevel.Instance.Reset();
        }

        private void OnExitMenu()
        {
            _poolContainer.ResetObjects();
            _mainHero.HidePlayer();
            _boundsController.ActivateBounds(false);
            _blur.focalLength.value = _blurValue;
            if (GameLevel.Instance.Points > GameData.Instance.SaveData.scores)
            {
                GameData.Instance.Save(GameLevel.Instance.Points);
                _uiMenu.ShowBestPoints(GameData.Instance.SaveData.scores > 0);
                _uiMenu.UpdateBestBoint(GameData.Instance.SaveData.scores);
            } 
        }

        private void OnPauseBegin(bool pause)
        {
            _poolContainer.PauseAllObjects(pause);
        }

        private void OnResumeCommercial()
        {
            _poolContainer.ResetSaveAndEnemy();
        }

        private void OnKeyboardEsc(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (GameLoop.Instance.GameState == GameState.GamePlay)
                GameLoop.Instance.Pause();
        }

        private void GameOver()
        {
            _poolContainer.PauseAllObjects(true);
        }

        private void BlurControl()
        {
            if (GameLoop.Instance.GameState is GameState.Pause or GameState.GameOver)
            {
                if (_blur.focalLength.value <= _blurValue)
                {
                    _blur.focalLength.value = Mathf.MoveTowards(_blur.focalLength.value, _blurValue, 0.8f);
                    return;
                }
                _blur.focalLength.value = _blurValue;
            }
            else
            {
                if (_blur.focalLength.value >= 1f)
                {
                    _blur.focalLength.value = Mathf.MoveTowards(_blur.focalLength.value, 1f, 5f);
                    return;
                }
                _blur.focalLength.value = 1f;
            }
        }
        #endregion
    }
}
