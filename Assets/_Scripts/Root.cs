using System.Collections.Generic;
using TouchFall.Controller;
using TouchFall.Controller.Interfaces;
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
        private List<IUpdater> _updaters = new();
        private List<IFixedUpdater> _fixeUpdaters = new();
        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            _mainHeroModel ??= new MainHeroModel();
        }

        private void Awake()
        {
            _mainHero.transform.position = _startPointHero.position;
            _mainHeroController = new(_mainHero, _mainHeroModel, _startPointHero.position);

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
