using System;
using UnityEngine;

namespace TouchFall.Model
{
    [Serializable]
    public sealed class BoundModel
    {
        #region Fields
        [Header("Top Bound")]
        [SerializeField] private float _topBoundPositionOffset = 5f;
        [Header("Side Bounds")]
        [SerializeField] private float _distanceBetweenBounds = 3f;
        [SerializeField] private float _boundsVerticalOffset = 2.5f;
        [SerializeField] private float _speedMove = 2.5f;
        [Header("Bottom Trigger")]
        [SerializeField] private float _bottomTriggerPositionOffset = 4f;
        [SerializeField] private float _bottomTriggerWidth = 30f;
        #endregion

        #region Properties
        public float TopBoundPositionOffset => _topBoundPositionOffset;
        public float DistnaceBetweenBounds => _distanceBetweenBounds;
        public float BoundsVerticalOffset => _boundsVerticalOffset;
        public float BottomTriggerPositionOffset => _bottomTriggerPositionOffset;
        public float BottomTriggerWidth => _bottomTriggerWidth;
        public float SpeedMove => _speedMove;
        #endregion
    }
}
