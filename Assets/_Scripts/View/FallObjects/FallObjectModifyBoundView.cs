using TouchFall.Helper.Enums;
using TouchFall.Singletons;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class FallObjectModifyBoundView : BaseFallObjectView, IScore
    {
        #region Fields
        [SerializeField] private ModifyBounds _modifyBounds;
        [SerializeField] private int _score;
        #endregion

        #region Properties
        public ModifyBounds ModifyBounds => _modifyBounds;
        public int Score => _score;
        #endregion

        #region Public Methods
        public override void ApplyMod()
        {
            ModifyBound.Instance.ApplyModify(_modifyBounds);
            gameObject.SetActive(false);
        }

        public override void DropObject()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
