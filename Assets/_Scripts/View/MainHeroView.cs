using System.Collections.Generic;
using TouchFall.Helper.Enums;
using TouchFall.Singletons;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class MainHeroView : MonoBehaviour
    {
        #region Fields
        [SerializeField] private HeroModifyType[] _bodies;

        private Rigidbody2D _currentBodyHero;
        private HeroModifyType _currentHero;
        private Transform _currentTransformHero;
        private AudioModify _audioHero;

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
            GameLevel.Instance.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            ModifyPlayer.Instance.Modify -= OnModify;
            GameLoop.Instance.PauseBegin -= Pause;
            GameLevel.Instance.GameOver -= OnGameOver;
        }
        #endregion

        #region Public Methods
        public void InstantiateHeroes(Vector2 position, AudioModify audioHero)
        {
            for (int i = 0; i < _bodies.Length; i++)
            {
                HeroModifyType typeHero = _bodies[i];
                typeHero = Instantiate(typeHero, position, Quaternion.identity);
                _modifiersHeroes.Add(typeHero.ModifyHero, typeHero);
                typeHero.gameObject.SetActive(false);
            }
            _currentModify = ModifyHero.Drop;
            _currentHero = _modifiersHeroes[_currentModify];
            _currentBodyHero = _currentHero.Body;           
            _currentTransformHero = _currentHero.transform;
            _audioHero = audioHero;
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
            if (_currentModify == modifyHero)
            {
                GameAudio.instance.PlayEmptyBonus();
                return;
            } 

            AudioClip clip = _audioHero.GetAudio(modifyHero);

            GameAudio.instance.PlaySoundHero(clip);

            ModifyPlayerStart(modifyHero);
        }

        private void ModifyPlayerStart(ModifyHero modifyHero)
        {
            _currentModify = modifyHero;
            Vector2 lastPos = _currentTransformHero.position;
            DeactivateAllHero();

            _currentHero = _modifiersHeroes[modifyHero];
            _currentBodyHero = _currentHero.Body;
            if (_currentHero != null)
            {
                _currentBodyHero.angularVelocity = 0f;
                _currentHero.transform.position = lastPos;
                _currentHero.gameObject.SetActive(true);
                _currentTransformHero = _currentHero.transform;
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

        private void OnGameOver()
        {
            Pause(true);
        }
        #endregion
    }
}

