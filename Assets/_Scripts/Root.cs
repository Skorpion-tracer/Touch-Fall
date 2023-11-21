using System.Collections.Generic;
using TouchFall.Controller;
using TouchFall.Controller.Interfaces;
using TouchFall.Helper;
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
        [SerializeField] private BoundView _boundView;
        [SerializeField] private BoundModel _boundModel;
        [SerializeField] private BottomTriggerView _bottomTriggerView;
        #endregion

        #region Fields
        private MainHeroController _mainHeroController;
        private BoundsController _boundsController;
        private PlayerControl _playerControl;
        private List<IUpdater> _updaters = new();
        private List<IFixedUpdater> _fixeUpdaters = new();

        private Vector2 _screenBounds;
        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            _mainHeroModel ??= new MainHeroModel();
            _boundModel ??= new BoundModel();
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

            BoundView leftBound = Instantiate(_boundView, Vector2.zero, Quaternion.identity);
            BoundView rightBound = Instantiate(_boundView, Vector2.zero, Quaternion.identity);

            _boundsController = new(_screenBounds, leftBound, rightBound, _boundModel, _postionTopBound);

            _bottomTriggerView.Initialized(_boundModel, _screenBounds);

            _mainHero = Instantiate(_mainHero, _startPointHero.position, Quaternion.identity);
            _mainHeroController = new(_mainHero, _mainHeroModel, _playerControl, _startPointHero.position);

            _updaters.Add(_mainHeroController);
            _updaters.Add(_boundsController);

            _fixeUpdaters.Add(_mainHeroController);
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
