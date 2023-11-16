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

        #region Properties
        public Rigidbody2D Body => _body;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
        }
        #endregion
    }
}

