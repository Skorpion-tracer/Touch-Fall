using TouchFall.View;
using UnityEngine;

namespace TouchFall.Helper.PoolObject
{
    public sealed class PoolEmptyObject : ObjectPool<FallObjectView>
    {
        #region Fields
        [SerializeField] private FallObjectView _fallObjectPrefab;
        #endregion

        #region Public Methods
        public override void InitPool() => SetPool(_fallObjectPrefab);

        public override FallObjectView GetPooledObject() => GetObject();
        #endregion
    }
}
