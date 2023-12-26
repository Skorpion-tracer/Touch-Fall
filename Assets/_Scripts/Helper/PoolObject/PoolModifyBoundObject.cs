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

        private const int _countObjectType = 3;
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
            int step = _count / _countObjectType;
            for (int i = 0; i < step; i++)
            {
                SetPool(_objectStay);
            }
            for (int i = 0; i < step; i++)
            {
                SetPool(_objectMove);
            }
            for (int i = 0; i < step; i++)
            {
                SetPool(_objectDistance);
            }
        }

        public override FallObjectModifyBoundView GetPooledObject()
        {
            FallObjectModifyBoundView fallObject = _pooledObjects[UnityEngine.Random.Range(0, _pooledObjects.Count - 1)];

            if (!fallObject.gameObject.activeInHierarchy)
            {
                return fallObject;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
