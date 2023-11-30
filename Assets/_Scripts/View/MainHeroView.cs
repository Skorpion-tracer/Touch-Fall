using System.Collections.Generic;
using TouchFall.Helper.Enums;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class MainHeroView : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Rigidbody2D[] _bodies;

        private Rigidbody2D _currentBodyHero;
        private Transform _currentTransformHero;

        private Dictionary<ModifyHero, HeroModifyType> _modifiersHeroes = new(6);

        private ModifyHero _currentModify;
        #endregion

        #region Properties
        public Rigidbody2D Body => _currentBodyHero;
        public Transform Transform => _currentTransformHero;
        #endregion

        #region Unity Methods
        //private void Awake()
        //{
        //    for (int i = 0; i < _bodies.Length; i++)
        //    {
        //        HeroModifyType typeHero = _bodies[i].GetComponent<HeroModifyType>();
        //        _modifiersHeroes.Add(typeHero.ModifyHero, typeHero);
        //        typeHero.gameObject.SetActive(false);
        //    }
        //    _currentBodyHero = _modifiersHeroes[ModifyHero.Drop].Body;
        //}

        private void OnEnable()
        {
            SingleModifyPlayer.Instance.Modify += OnModify;
        }

        private void OnDisable()
        {
            SingleModifyPlayer.Instance.Modify -= OnModify;
        }
        #endregion

        #region Public Methods
        public void InstantiateHeroes(Vector2 position)
        {
            for (int i = 0; i < _bodies.Length; i++)
            {
                HeroModifyType typeHero = _bodies[i].GetComponent<HeroModifyType>();
                typeHero = Instantiate(typeHero, position, Quaternion.identity);
                _modifiersHeroes.Add(typeHero.ModifyHero, typeHero);
                typeHero.gameObject.SetActive(false);
            }
            _currentModify = ModifyHero.Drop;
            _currentBodyHero = _modifiersHeroes[_currentModify].Body;
            _currentBodyHero.gameObject.SetActive(true);
            _currentTransformHero = _currentBodyHero.gameObject.transform;
        }
        #endregion

        #region Private Methods
        private void OnModify(ModifyHero modifyHero)
        {
            if (_currentModify == modifyHero) return;

            _currentModify = modifyHero;
            Vector2 lastPos = _currentTransformHero.position;
            DeactivateAllHero();

            _currentBodyHero = _modifiersHeroes[modifyHero].Body;
            if (_currentBodyHero != null)
            {
                _currentBodyHero.transform.position = lastPos;
                _currentBodyHero.gameObject.SetActive(true);
                _currentTransformHero = _currentBodyHero.gameObject.transform;
            }
        }

        private void DeactivateAllHero()
        {
            foreach (KeyValuePair<ModifyHero, HeroModifyType> modifiers in _modifiersHeroes)
            {
                modifiers.Value.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}

