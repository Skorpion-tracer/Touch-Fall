using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public sealed class FallObjectView : MonoBehaviour, IFallingObject
    {
        #region Public Methods
        public void ApplyMod()
        {
            Debug.Log("Применить модификатор");
            SingleModifyPlayer.Instance.ApplyModify();
        }

        public void DropObject()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
