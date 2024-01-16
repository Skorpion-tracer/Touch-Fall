using TouchFall.View;

namespace TouchFall.Helper.PoolObject
{
    public sealed class PoolContainer
    {
        #region Fields
        private ObjectPool<FallObjectView> _poolEmptyObjects;
        private ObjectPool<FallObjectModifyHeroView> _poolModifyObjects;
        private ObjectPool<FallObjectModifyBoundView> _poolModifyBoundObjects;
        private ObjectPool<FallObjectEnemyView> _poolEnemyObjects;
        private ObjectPool<FallObjectNeedToSaveView> _poolNeedToSaveObjects;
        #endregion

        #region Constructor
        public PoolContainer(ObjectPool<FallObjectView> poolEmptyObjects, 
            ObjectPool<FallObjectModifyHeroView> poolModifyObjects, 
            ObjectPool<FallObjectModifyBoundView> poolModifyBound,
            ObjectPool<FallObjectEnemyView> poolEnemyObjects,
            ObjectPool<FallObjectNeedToSaveView> poolNeedToSaveObjects)
        {
            _poolEmptyObjects = poolEmptyObjects;
            _poolModifyObjects = poolModifyObjects;
            _poolModifyBoundObjects = poolModifyBound;
            _poolEnemyObjects = poolEnemyObjects;
            _poolNeedToSaveObjects = poolNeedToSaveObjects;
        }
        #endregion

        #region Properties
        public ObjectPool<FallObjectView> PoolEmptyObjects => _poolEmptyObjects;
        public ObjectPool<FallObjectModifyHeroView> PoolModifyObjects => _poolModifyObjects;
        public ObjectPool<FallObjectModifyBoundView> PoolModifyBoundObjects => _poolModifyBoundObjects;
        public ObjectPool<FallObjectEnemyView> PoolEnemyObjects => _poolEnemyObjects;
        public ObjectPool<FallObjectNeedToSaveView> PoolNeedToSaveObjects => _poolNeedToSaveObjects;
        #endregion

        #region Public Methods
        public void ResetObjects()
        {
            PoolEmptyObjects.ResetAllObjects();
            PoolModifyObjects.ResetAllObjects();
            PoolModifyBoundObjects.ResetAllObjects();
            PoolEnemyObjects.ResetAllObjects();
            PoolNeedToSaveObjects.ResetAllObjects();
        }
        #endregion
    }
}
