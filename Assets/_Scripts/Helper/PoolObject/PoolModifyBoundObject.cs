using System;
using TouchFall.Helper.Enums;
using TouchFall.View;
using UnityEngine;

namespace TouchFall.Helper.PoolObject
{
    public sealed class PoolModifyBoundObject : ObjectPool<FallObjectModifyBoundView>
    {
        #region Fields
        [SerializeField] private FallObjectModifyBoundView _objectStay;
        [SerializeField] private FallObjectModifyBoundView _objectMove;
        [SerializeField] private FallObjectModifyBoundView _objectDistance;
        #endregion

        #region Unity Methods
        private void OnValidate()
        {
            if (_objectStay.ModifyBounds == ModifyBounds.Stay &&
                _objectMove.ModifyBounds == ModifyBounds.Moving &&
                _objectDistance.ModifyBounds == ModifyBounds.IncreaseDistance) return;
            throw new ArgumentException("Обьекты не соответствуют модификаторам");
        }
        #endregion

        #region Public Methods
        public override void InitPool()
        {
            throw new NotImplementedException();
        }

        public override FallObjectModifyBoundView GetPooledObject()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
