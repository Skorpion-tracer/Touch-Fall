using TouchFall.View;

namespace TouchFall.Helper.PoolObject
{
    public sealed class PoolContainer
    {
        #region Fields
        private ObjectPool<FallObjectView> _poolEmptyObjects;
        private ObjectPool<FallObjectModifyHeroView> _poolModifyObjects;
        private ObjectPool<FallObjectModifyBoundView> _poolModifyBoundObjects;
        #endregion

        #region Constructor
        public PoolContainer(ObjectPool<FallObjectView> poolEmptyObjects, ObjectPool<FallObjectModifyHeroView> poolModifyObjects, ObjectPool<FallObjectModifyBoundView> poolModifyBound)
        {
            _poolEmptyObjects = poolEmptyObjects;
            _poolModifyObjects = poolModifyObjects;
            _poolModifyBoundObjects = poolModifyBound;
        }
        #endregion

        #region Properties
        public ObjectPool<FallObjectView> PoolEmptyObjects => _poolEmptyObjects;
        public ObjectPool<FallObjectModifyHeroView> PoolModifyObjects => _poolModifyObjects;
        public ObjectPool<FallObjectModifyBoundView> PoolModifyBoundObjects => _poolModifyBoundObjects;
        #endregion
    }
}
