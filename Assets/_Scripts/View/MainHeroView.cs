using System;
using UnityEngine;

namespace TouchFall.View
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class MainHeroView : MonoBehaviour
    {
        #region Fields
        private Rigidbody2D _body;
        #endregion

        #region Events
        public event Action EnableView;
        public event Action DisableView;
        #endregion

        #region Properties
        public Rigidbody2D Body => _body;
        #endregion

        #region Unity Methods
        private void OnValidate()
        {
            _body ??= GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            EnableView?.Invoke();
        }

        private void OnDisable()
        {
            DisableView?.Invoke();
        }
        #endregion
    }
}

