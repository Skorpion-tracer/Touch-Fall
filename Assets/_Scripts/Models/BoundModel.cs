using System;
using UnityEngine;

namespace TouchFall.Model
{
    [Serializable]
    public sealed class BoundModel
    {
        #region Fields
        private float _minDistanceBetweenBounds = 1.85f;

        [Header("Top Bound")]
        [SerializeField] private float _topBoundPositionOffset = 5f;
        [Header("Side Bounds")]
        [SerializeField] private float _distanceBetweenBounds = 3f;
        [SerializeField] private float _boundsVerticalOffset = 2.5f;
        [SerializeField] private float _speedMove = 2.5f;
        [SerializeField] private float _coeffDecrease = 0.1f;
        [Header("Bottom Trigger")]
        [SerializeField] private float _bottomTriggerPositionOffset = 4f;
        [SerializeField] private float _bottomTriggerWidth = 30f;
        #endregion

        #region Properties
        public float TopBoundPositionOffset => _topBoundPositionOffset;
        public float DistnaceBetweenBounds
        {
            get => _distanceBetweenBounds;
            set
            {
                if (value <= _minDistanceBetweenBounds)
                {
                    value = _minDistanceBetweenBounds;
                }
                _distanceBetweenBounds = value;
            }
        }
        public float BoundsVerticalOffset => _boundsVerticalOffset;
        public float BottomTriggerPositionOffset => _bottomTriggerPositionOffset;
        public float BottomTriggerWidth => _bottomTriggerWidth;
        public float SpeedMove => _speedMove;
        #endregion

        #region Public Methods
        public void DecreaseDistanceBound()
        {
            DistnaceBetweenBounds -= _coeffDecrease;
        }
        #endregion
    }
}
