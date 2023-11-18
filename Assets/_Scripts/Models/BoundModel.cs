using System;
using UnityEngine;

namespace TouchFall.Model
{
    [Serializable]
    public sealed class BoundModel
    {
        #region Fields
        [SerializeField] private float _topBoundPositionOffset = 5f;
        [SerializeField] private float _distanceBetweenBounds = 3f;
        #endregion

        #region Properties
        public float TopBoundPositionOffset => _topBoundPositionOffset;
        public float DistnaceBetweenBounds => _distanceBetweenBounds;
        #endregion
    }
}
