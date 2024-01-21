using System.Collections.Generic;
using TouchFall.Helper.Enums;
using TouchFall.Singletons;
using TouchFall.View.Interfaces;
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
        private Vector2 _lastVelocity;
        private float _lastAngularVelocity;
        #endregion

        #region Properties
        public Rigidbody2D Body => _currentBodyHero;
        public Transform Transform => _currentTransformHero;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            ModifyPlayer.Instance.Modify += OnModify;
            GameLoop.Instance.PauseBegin += Pause;
        }

        private void OnDisable()
        {
            ModifyPlayer.Instance.Modify -= OnModify;
            GameLoop.Instance.PauseBegin -= Pause;
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
            _currentTransformHero = _currentBodyHero.gameObject.transform;
        }

        public void HidePlayer()
        {
            _modifiersHeroes[_currentModify].gameObject.SetActive(false);
        }

        public void ResetPlayer(Vector2 position)
        {
            ModifyPlayerStart(ModifyHero.Drop);
            _currentBodyHero.transform.position = position;
        }

        public void DeactivateAllHero()
        {
            foreach (KeyValuePair<ModifyHero, HeroModifyType> modifiers in _modifiersHeroes)
            {
                modifiers.Value.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Private Methods
        private void OnModify(ModifyHero modifyHero)
        {
            if (_currentModify == modifyHero) return;

            ModifyPlayerStart(modifyHero);
        }

        private void ModifyPlayerStart(ModifyHero modifyHero)
        {
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

        private void Pause(bool isPause)
        {
            if (isPause)
            {
                _lastVelocity = _currentBodyHero.velocity;
                _lastAngularVelocity = _currentBodyHero.angularVelocity;
                _currentBodyHero.velocity = Vector2.zero;
                _currentBodyHero.angularVelocity = 0f;
                _currentBodyHero.isKinematic = isPause;
            }
            else
            {
                _currentBodyHero.isKinematic = isPause;
                _currentBodyHero.velocity = _lastVelocity;
                _currentBodyHero.angularVelocity = _lastAngularVelocity;
            }
        }
        #endregion
    }
}

