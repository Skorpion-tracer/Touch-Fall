using TouchFall.Singletons;
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
            Debug.Log("<color=Green>Объект спасён</color>");
            gameObject.SetActive(false);
        }

        public void DropObject()
        {
            Debug.Log($"Не удалось спасти. <color=Red>Минус жизнь!!!</color>");
            GameLevel.Instance.ApplyDamage();
            gameObject.SetActive(false);
        }
        #endregion
    }
}
