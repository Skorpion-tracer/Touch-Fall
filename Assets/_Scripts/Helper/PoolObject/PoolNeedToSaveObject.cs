using TouchFall.View;
using UnityEngine;

namespace TouchFall.Helper.PoolObject
{
    public sealed class PoolNeedToSaveObject : ObjectPool<FallObjectNeedToSaveView>
    {
        #region Fields
        [SerializeField] private FallObjectNeedToSaveView _enemyObject;
        #endregion

        #region Public Methods
        public override void InitPool() => SetPool(_enemyObject);

        public override FallObjectNeedToSaveView GetPooledObject() => GetObject();
        #endregion
    }
}
