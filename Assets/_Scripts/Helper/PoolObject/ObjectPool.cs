using System.Collections.Generic;
using TouchFall.View;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.Helper.PoolObject
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IFallingObject
    {
        #region Fields
        [SerializeField] protected int _count = 20;

        protected List<T> _pooledObjects = new();        
        #endregion

        #region Public Methods
        public abstract void InitPool();

        public abstract T GetPooledObject();
        #endregion

        #region Protected Methods
        protected void SetPool(T objectView)
        {
            for (int i = 0; i < _count; i++)
            {
                T fallObject = Instantiate(objectView);
                fallObject.transform.SetParent(transform);
                fallObject.gameObject.SetActive(false);
                _pooledObjects.Add(fallObject);
            }
        }

        protected T GetObject()
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
