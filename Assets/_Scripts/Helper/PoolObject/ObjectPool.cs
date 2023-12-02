using System.Collections.Generic;
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
    }
}
