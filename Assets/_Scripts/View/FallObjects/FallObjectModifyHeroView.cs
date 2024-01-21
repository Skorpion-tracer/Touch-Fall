using TouchFall.Helper.Enums;
using TouchFall.Singletons;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class FallObjectModifyHeroView : BaseFallObjectView, IScore
    {
        #region Fields
        [SerializeField] private ModifyHero _modifyHero;
        [SerializeField] private int _score;
        #endregion

        #region Properties
        public int Score => _score;
        #endregion

        #region Public Methods
        public override void ApplyMod()
        {
            ModifyPlayer.Instance.ApplyModify(_modifyHero);
            gameObject.SetActive(false);
        }

        public override void DropObject()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
