using TouchFall.View;

namespace TouchFall.Helper.PoolObject
{
    public sealed class PoolContainer
    {
        #region Fields
        private ObjectPool<FallObjectView> _poolEmptyObjects;
        private ObjectPool<FallObjectModifyView> _poolModifyObjects;
        #endregion

        #region Constructor
        public PoolContainer(ObjectPool<FallObjectView> poolEmptyObjects, ObjectPool<FallObjectModifyView> poolModifyObjects)
        {
            _poolEmptyObjects = poolEmptyObjects;
            _poolModifyObjects = poolModifyObjects;
        }
        #endregion

        #region Properties
        public ObjectPool<FallObjectView> PoolEmptyObjects => _poolEmptyObjects;
        public ObjectPool<FallObjectModifyView> PoolModifyObjects => _poolModifyObjects;
        #endregion
    }
}
