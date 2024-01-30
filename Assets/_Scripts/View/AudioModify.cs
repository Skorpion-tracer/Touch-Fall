using TouchFall.Helper.Enums;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class AudioModify : MonoBehaviour
    {
        #region Serialize Fileds
        [Header("Hero")]
        [SerializeField] private AudioClip _heroSound;
        [SerializeField] private AudioClip _softHeroSound;
        [SerializeField] private AudioClip _magneticHeroSound;
        [SerializeField] private AudioClip _repulseHeroSound;
        [SerializeField] private AudioClip _spinHeroSound;
        [SerializeField] private AudioClip _squareHeroSound;

        [Space(5f), Header("Bounds")]
        [SerializeField] private AudioClip _boundReset;
        [SerializeField] private AudioClip _boundMove;
        [SerializeField] private AudioClip _boundIncrease;
        #endregion

        #region Public Methods
        public AudioClip GetAudio(ModifyHero modifyHero)
        {
            switch (modifyHero)
            {
                case ModifyHero.Drop: return _heroSound;
                case ModifyHero.Elastic: return _softHeroSound;
                case ModifyHero.Magnetic: return _magneticHeroSound;
                case ModifyHero.Repulsive: return _repulseHeroSound;
                case ModifyHero.Spinning: return _spinHeroSound;
                case ModifyHero.Square: return _squareHeroSound;
            }
            return null;
        }

        public AudioClip GetAudio(ModifyBounds modifyHero)
        {
            switch (modifyHero)
            {
                case ModifyBounds.Moving: return _boundMove;
                case ModifyBounds.IncreaseDistance: return _boundIncrease;
                case ModifyBounds.Stay: return _boundReset;
            }
            return null;
        }
        #endregion
    }
}