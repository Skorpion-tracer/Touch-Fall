using TouchFall.View;
using UnityEngine;

namespace TouchFall.Helper.PoolObject
{
    public sealed class PoolModifyObject : ObjectPool<FallObjectModifyHeroView>
    {
        #region Fields
        [SerializeField] private FallObjectModifyHeroView[] _fallObjectsPrefab;
        #endregion

        #region Public Methods
        public override void InitPool()
        {
            int step = _count / _fallObjectsPrefab.Length;
            Debug.Log(step);
            int j = 0;
            for (int i = 0; i <= _count; i++)
            {
                InitIobject(_fallObjectsPrefab[j]);
                if (i == 0) i++;
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

        public override FallObjectModifyHeroView GetPooledObject()
        {
            FallObjectModifyHeroView fallObject = _pooledObjects[Random.Range(0, _pooledObjects.Count - 1)];

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