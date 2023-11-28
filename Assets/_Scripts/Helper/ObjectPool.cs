using System.Collections.Generic;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.Helper
{
    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IFallingObject
    {
        #region Fields
        [SerializeField] private T _fallObjectPrefab;

        private List<T> _pooledObjects = new();
        private int _count = 20;
        #endregion

        #region Public Methods
        public void InitPool()
        {
            for (int i = 0; i < _count; i++)
            {
                T fallObject = Instantiate(_fallObjectPrefab);
                fallObject.gameObject.SetActive(false);
                _pooledObjects.Add(fallObject);
            }
        }

        public T GetPooledObject()
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
