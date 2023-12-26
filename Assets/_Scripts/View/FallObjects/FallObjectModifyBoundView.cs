using TouchFall.Helper.Enums;
using TouchFall.Singletons;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class FallObjectModifyBoundView : MonoBehaviour, IFallingObject
    {
        #region Fields
        [SerializeField] private ModifyBounds _modifyBounds;
        #endregion

        #region Properties
        public ModifyBounds ModifyBounds => _modifyBounds;
        #endregion

        #region Public Methods
        public void ApplyMod()
        {
            ModifyBound.Instance.ApplyModify(_modifyBounds);
            gameObject.SetActive(false);
        }

        public void DropObject()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
