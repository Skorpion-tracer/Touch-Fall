using TouchFall.View;
using UnityEngine;

namespace TouchFall.Helper.PoolObject
{
    public sealed class PoolModifyObject : ObjectPool<FallObjectModifyView>
    {
        #region Fields
        [SerializeField] private FallObjectModifyView[] _fallObjectsPrefab;
        #endregion

        #region Public Methods
        public override void InitPool()
        {
            int step = _count / _fallObjectsPrefab.Length;
            int j = 0;
            for (int i = 0; i < _count; i++)
            {

                FallObjectModifyView fallObject = Instantiate(_fallObjectsPrefab[j]);
                fallObject.transform.SetParent(transform);
                fallObject.gameObject.SetActive(false);
                _pooledObjects.Add(fallObject);

                if (i > 0)
                {
                    if (_fallObjectsPrefab.Length > 0 && i % step == 0 && j < _fallObjectsPrefab.Length)
                    {
                        j++;
                        if (j >= _fallObjectsPrefab.Length) j = _fallObjectsPrefab.Length - 1;
                    }
                }
            }
        }

        public override FallObjectModifyView GetPooledObject()
        {
            FallObjectModifyView fallObject = _pooledObjects[Random.Range(0, _pooledObjects.Count - 1)];

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