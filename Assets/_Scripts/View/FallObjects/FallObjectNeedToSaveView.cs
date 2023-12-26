using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public sealed class FallObjectNeedToSaveView : MonoBehaviour, IFallingObject
    {
        #region Public Methods
        public void ApplyMod()
        {
            Debug.Log("Объект спасён");
            gameObject.SetActive(false);
        }

        public void DropObject()
        {
            Debug.Log($"Не удалось спасти. <color=Red>Минус жизнь!!!</color>");
            gameObject.SetActive(false);
        }
        #endregion
    }
}
