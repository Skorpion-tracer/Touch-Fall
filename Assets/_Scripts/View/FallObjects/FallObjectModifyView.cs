using TouchFall.Helper.Enums;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class FallObjectModifyView : MonoBehaviour, IFallingObject
    {
        #region Fields
        [SerializeField] private ModifyHero _modifyHero;
        #endregion

        #region Unity Methods
        private void Start()
        {
            
        }
        #endregion

        #region Public Methods
        public void ApplyMod()
        {
            Debug.Log($"Изменить поведение игрока: {_modifyHero}");

            SingleModifyPlayer.Instance.ApplyModify(_modifyHero);
        }

        public void DropObject()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
