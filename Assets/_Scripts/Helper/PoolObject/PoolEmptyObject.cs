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
        public override void InitPool()
        {
            for (int i = 0; i < _count; i++)
            {
                FallObjectView fallObject = Instantiate(_fallObjectPrefab);
                fallObject.transform.SetParent(transform);
                fallObject.gameObject.SetActive(false);
                _pooledObjects.Add(fallObject);
            }
        }

        public override FallObjectView GetPooledObject()
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].gameObject.activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }

            return null;
        }
        #endregion
    }
}
