using System.Collections.Generic;
using TouchFall.Controller;
using TouchFall.Controller.Interfaces;
using TouchFall.Input;
using TouchFall.View;
using UnityEngine;

namespace TouchFall
{
    public sealed class Root : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private MainHeroView _mainHero;
        [SerializeField] private MainHeroModel _mainHeroModel;
        [SerializeField] private Transform _startPointHero;
        #endregion

        #region Fields
        private MainHeroController _mainHeroController;
        private PlayerControl _playerControl;
        private List<IUpdater> _updaters = new();
        private List<IFixedUpdater> _fixeUpdaters = new();
        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            _mainHeroModel ??= new MainHeroModel();
        }

        private void OnEnable()
        {
            _playerControl.Enable();
        }

        private void OnDisable()
        {
            _playerControl.Disable();
        }

        private void Awake()
        {
            _playerControl = new();

            _mainHero = Instantiate(_mainHero, _startPointHero.position, Quaternion.identity);
            _mainHeroController = new(_mainHero, _mainHeroModel, _playerControl, _startPointHero.position);

            _updaters.Add(_mainHeroController);
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
