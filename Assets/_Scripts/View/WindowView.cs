using System;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class WindowView : MonoBehaviour
    {
        #region Events
        public event Action Caught;
        #endregion

        #region UnityMethods
        private void OnTriggerEnter2D(Collider2D collision) //TODO: реализовать проверку на падающий объект
        {
            //if (collision.TryGetComponent<FallObject>())
            //{
            //    Caught?.Invoke();
            //}
        }
        #endregion
    }
}
