using System.Collections.Generic;
using TouchFall.View;
using UnityEngine;

namespace TouchFall.Helper
{
    public sealed class ObjectPool : MonoBehaviour
    {
        #region Fields
        [SerializeField] private FallObjectView _fallObjectPrefab;

        private List<FallObjectView> _pooledObjects = new();
        private int _count = 20;
        #endregion

        #region Public Methods
        public void InitPool()
        {
            for (int i = 0; i < _count; i++)
            {
                FallObjectView fallObject = Instantiate(_fallObjectPrefab);
                fallObject.gameObject.SetActive(false);
                _pooledObjects.Add(fallObject);
            }
        }

        public FallObjectView GetPooledObject()
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
