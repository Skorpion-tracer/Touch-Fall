using System.Collections.Generic;
using TouchFall.Controller;
using TouchFall.Controller.Interfaces;
using TouchFall.Helper;
using TouchFall.Helper.PoolObject;
using TouchFall.Input;
using TouchFall.Model;
using TouchFall.View;
using UnityEngine;

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
        [SerializeField] private SpawnFallObjectModel _spawnModel;
        #endregion

        #region Fields
        private MainHeroMoveController _mainHeroMoveController;
        private MainHeroBehavoiurController _mainHeroBehavoiurController;
        private BoundsController _boundsController;
        private PlayerControl _playerControl;
        private SpawnFallObjectController _spawnController;
        private List<IUpdater> _updaters = new();
        private List<IFixedUpdater> _fixeUpdaters = new();
        private PoolContainer _poolContainer;

        private Vector2 _screenBounds;
        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            _mainHeroModel ??= new MainHeroModel();
            _boundModel ??= new BoundModel();
            _spawnModel ??= new SpawnFallObjectModel();
        }

        private void OnEnable()
        {
            _playerControl?.Enable();
        }

        private void OnDisable()
        {
            _playerControl?.Disable();
        }

        private void Awake()
        {
            _playerControl = new();

            _screenBounds = Utils.ScreenToWorld(Camera.main, new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

            _poolEmptyObject.InitPool();
            _poolModifyObject.InitPool();

            _poolContainer = new(_poolEmptyObject, _poolModifyObject);

            BoundView leftBound = Instantiate(_leftBoundView, Vector2.zero, Quaternion.identity);
            BoundView rightBound = Instantiate(_rightBoundView, Vector2.zero, Quaternion.identity);

            _boundsController = new(_screenBounds, leftBound, rightBound, _boundModel, _postionTopBound);

            _bottomTriggerView.Initialized(_boundModel, _screenBounds);

            _mainHero.InstantiateHeroes(_startPointHero.position); //= Instantiate(_mainHero, _startPointHero.position, Quaternion.identity);
            _mainHeroMoveController = new(_mainHero, _mainHeroModel, _playerControl, _startPointHero.position, _screenBounds);
            _mainHeroBehavoiurController = new(_mainHero, _mainHeroModel);

            _spawnController = new(_spawnModel, _poolContainer, _screenBounds);

            _updaters.Add(_mainHeroMoveController);
            _updaters.Add(_boundsController);
            _updaters.Add(_spawnController);

            _fixeUpdaters.Add(_mainHeroMoveController);
            _fixeUpdaters.Add(_mainHeroBehavoiurController);
        }

        private void Update()
        {
            for (int i = 0; i < _updaters.Count; i++)
            {
                _updaters[i].Update();
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _fixeUpdaters.Count; i++)
            {
                _fixeUpdaters[i].FixedUpdate();
            }
        }
        #endregion
    }
}
