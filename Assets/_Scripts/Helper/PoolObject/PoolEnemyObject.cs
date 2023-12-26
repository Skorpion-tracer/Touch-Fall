using TouchFall.View;
using UnityEngine;

namespace TouchFall.Helper.PoolObject
{
    public sealed class PoolEnemyObject : ObjectPool<FallObjectEnemyView>
    {
        #region Fields
        [SerializeField] private FallObjectEnemyView _enemyObject;
        #endregion

        #region Public Methods
        public override void InitPool() => SetPool(_enemyObject);

        public override FallObjectEnemyView GetPooledObject() => GetObject();
        #endregion
    }
}
