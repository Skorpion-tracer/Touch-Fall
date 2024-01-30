using TouchFall.Singletons;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class FallObjectNeedToSaveView : BaseFallObjectView, IScore
    {
        #region Fields
        [SerializeField] private int _score;
        [SerializeField] private AudioClip _sound;
        #endregion

        #region Properties
        public int Score => _score;
        #endregion

        #region Public Methods
        public override void ApplyMod()
        {
            gameObject.SetActive(false);
            GameAudio.instance.PlaySound(_sound);
        }

        public override void DropObject()
        {
            GameLevel.Instance.ApplyDamage();
            gameObject.SetActive(false);
        }
        #endregion
    }
}
