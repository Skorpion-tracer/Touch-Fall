using System;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class WindowView : MonoBehaviour
    {
        #region Events
        public event Action Caught;
        #endregion

        #region UnityMethods
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IFallingObject fallingObject))
            {
                fallingObject.ApplyMod();
                fallingObject.DropObject();
            }
        }
        #endregion
    }
}
