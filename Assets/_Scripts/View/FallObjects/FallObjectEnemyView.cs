using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class FallObjectEnemyView : MonoBehaviour, IFallingObject
    {
        #region Public Methods
        public void ApplyMod()
        {
            Debug.Log($"<color=Red>Враг!!! Минус Жизнь!!!</color>");
            gameObject.SetActive(false);
        }

        public void DropObject()
        {
            Debug.Log("<color=Green>Миновали врага</color>");
            gameObject.SetActive(false);
        }
        #endregion
    }
}
