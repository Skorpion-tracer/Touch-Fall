using TouchFall.Helper.Enums;
using TouchFall.Singletons;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class FallObjectModifyHeroView : MonoBehaviour, IFallingObject
    {
        #region Fields
        [SerializeField] private ModifyHero _modifyHero;
        #endregion

        #region Public Methods
        public void ApplyMod()
        {
            ModifyPlayer.Instance.ApplyModify(_modifyHero);
        }

        public void DropObject()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
